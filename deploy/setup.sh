#!/usr/bin/env bash

set -e

cd "$(dirname "$(realpath "$0")")";

# TODO ensure required cli tools are installed / configured: gcloud (google), gh (github), atlas (mongodb) and cf (cloudflare)

# Load env variables
set -a
source .env
test -f .env.local && source .env.local
set +a

function slugify {
   iconv -t "ascii//TRANSLIT" | sed -r "s/[^a-zA-Z0-9]+/-/g" | sed -r "s/^-+\|-+$//g" | tr "[:upper:]" "[:lower:]"
}

# Env defaults
PROJECT_SLUG=$(echo "${PROJECT_NAME}" | slugify )
SERVICE_ACCOUNT_NAME="github-cicd"
IDENTITY_POOL_NAME="github-pool"
IDENTITY_PROVIDER_NAME="github-provider"

# Create project
if ! gcloud projects describe "${PROJECT_SLUG}" > /dev/null; then
  echo "Creating project: ${PROJECT_SLUG}"
  gcloud projects create "${PROJECT_SLUG}" --name "${PROJECT_NAME}"
else
  echo "Project already exists: ${PROJECT_SLUG}"
fi
GOOGLE_PROJECT_ID=$PROJECT_SLUG
echo ""

# Enable required services
echo "Enabling required services"
gcloud services enable iamcredentials.googleapis.com --project "${PROJECT_SLUG}"
gcloud services enable artifactregistry.googleapis.com --project "${PROJECT_SLUG}"
gcloud services enable run.googleapis.com --project "${PROJECT_SLUG}"
echo ""

# Link billing account
GOOGLE_BILLING_ACCOUNT_FIRST=$(gcloud beta billing accounts list --filter="open=true" --format="value(name)" --limit=1)
GOOGLE_BILLING_ACCOUNT_ID=${GOOGLE_BILLING_ACCOUNT_ID:-${GOOGLE_BILLING_ACCOUNT_FIRST}}
echo "Ensure project ${PROJECT_SLUG} is linked to billing account ${GOOGLE_BILLING_ACCOUNT_ID}"
gcloud beta billing projects link "${PROJECT_SLUG}" "--billing-account=${GOOGLE_BILLING_ACCOUNT_ID}"
echo ""

# Create identity pool
# @see (https://github.com/google-github-actions/auth#setup)
if ! gcloud iam workload-identity-pools describe --location="global" --project="${PROJECT_SLUG}" "${IDENTITY_POOL_NAME}" > /dev/null; then
    echo "Creating identity pool: ${IDENTITY_POOL_NAME}"
    gcloud iam workload-identity-pools create "${IDENTITY_POOL_NAME}" \
        --project="${PROJECT_SLUG}" \
        --location="global" \
        --display-name="${IDENTITY_POOL_NAME}"
else
    echo "Identity pool already exists: ${IDENTITY_POOL_NAME}"
fi
IDENTITY_POOL_ID=$(
    gcloud iam workload-identity-pools describe "${IDENTITY_POOL_NAME}" \
        --project="${PROJECT_SLUG}" \
        --location="global" \
        --format="value(name)"
)
echo ""

if ! gcloud iam workload-identity-pools providers describe \
    --workload-identity-pool="${IDENTITY_POOL_NAME}" \
    --location="global" \
    --project="${PROJECT_SLUG}" "${IDENTITY_PROVIDER_NAME}" > /dev/null
then
    echo "Creating identity pool provider: ${IDENTITY_PROVIDER_NAME}"
    gcloud iam workload-identity-pools providers create-oidc "${IDENTITY_PROVIDER_NAME}" \
        --project="${PROJECT_SLUG}" \
        --location="global" \
        --workload-identity-pool="${IDENTITY_POOL_NAME}" \
        --display-name="${IDENTITY_PROVIDER_NAME}" \
        --attribute-mapping="google.subject=assertion.sub,attribute.actor=assertion.actor,attribute.repository=assertion.repository" \
        --issuer-uri="https://token.actions.githubusercontent.com"
else
    echo "Identity pool provider already exists: ${IDENTITY_PROVIDER_NAME}"
fi
echo ""


# Add service account and permissions
GOOGLE_SERVICE_ACCOUNT_EMAIL="${SERVICE_ACCOUNT_NAME}@${PROJECT_SLUG}.iam.gserviceaccount.com"
if ! gcloud iam service-accounts describe --project="${PROJECT_SLUG}" "${GOOGLE_SERVICE_ACCOUNT_EMAIL}" > /dev/null 2>&1; then
    echo "Creating service account: ${SERVICE_ACCOUNT_NAME}"
    gcloud iam service-accounts create --project="${PROJECT_SLUG}" ${SERVICE_ACCOUNT_NAME}
else
    echo "Service account already exists: ${SERVICE_ACCOUNT_NAME}"
fi

echo "Setting gcloud roles"
gcloud iam service-accounts add-iam-policy-binding "${GOOGLE_SERVICE_ACCOUNT_EMAIL}" \
    --project="${PROJECT_SLUG}" \
    --role="roles/iam.workloadIdentityUser" \
    --member="principalSet://iam.googleapis.com/${IDENTITY_POOL_ID}/attribute.repository/${GITHUB_REPOSITORY}"
GCLOUD_ROLES=(
  "roles/artifactregistry.repoAdmin"
  "roles/run.developer"
  "roles/iam.serviceAccountUser"
)
for GCLOUD_ROLE in "${GCLOUD_ROLES[@]}"; do
	  gcloud projects add-iam-policy-binding "${PROJECT_SLUG}" \
        --role="${GCLOUD_ROLE}" \
        --member="serviceAccount:${GOOGLE_SERVICE_ACCOUNT_EMAIL}"
done
GOOGLE_WORKLOAD_IDENTITY_PROVIDER=$(
    gcloud iam workload-identity-pools providers describe "${IDENTITY_PROVIDER_NAME}" \
        --project="${PROJECT_SLUG}" \
        --location="global" \
        --workload-identity-pool="${IDENTITY_POOL_NAME}" \
        --format="value(name)"
)
echo ""


# Create container registry
if ! gcloud artifacts repositories describe --project="${PROJECT_SLUG}" --location="${GOOGLE_REGION}" docker > /dev/null; then
    echo "Creating artifact repository"
    gcloud artifacts repositories create docker \
        --repository-format=docker \
        --description="Docker repository" \
        --location="${GOOGLE_REGION}" \
        --project="${PROJECT_SLUG}"
else
    echo "Artifact repository already created"
fi
echo ""


# Create mongodb atlas project
if ! atlas project list | grep "${PROJECT_SLUG}" > /dev/null; then
    echo "Creating MongoDB Atlas project"
    atlas project create "${PROJECT_SLUG}"
else
    echo "MongoDB Atlas project already created"
fi
MONGODB_ATLAS_PROJECT_ID=$(atlas project list --output=json | jq -r ".results[] | select(.name == \"${PROJECT_SLUG}\") | .id")
echo "MongoDB project ID: ${MONGODB_ATLAS_PROJECT_ID}"
echo ""


# Recreate api key
MONGODB_ATLAS_API_KEY_ID=$(atlas project apiKeys list --projectId="${MONGODB_ATLAS_PROJECT_ID}" --output=json | jq -r ".[] | select(.desc == \"${SERVICE_ACCOUNT_NAME}\") | .id")
if [ -n "${MONGODB_ATLAS_API_KEY_ID}" ]; then
    echo "Delete old MongoDB Atlas api key ${MONGODB_ATLAS_API_KEY_ID}"
    atlas projects apiKeys delete --force "${MONGODB_ATLAS_API_KEY_ID}"
fi 

echo "Creating MongoDB Atlas api key"
MONGODB_ATLAS_API_KEY_CREATE_OUTPUT=$(
    atlas projects apiKeys create \
        --role=ORG_OWNER,ORG_GROUP_CREATOR \
        --output=json \
        --projectId="${MONGODB_ATLAS_PROJECT_ID}" \
        --desc="${SERVICE_ACCOUNT_NAME}"
)
echo "${MONGODB_ATLAS_API_KEY_CREATE_OUTPUT}"
echo ""

MONGODB_ATLAS_PUBLIC_KEY=$(echo "${MONGODB_ATLAS_API_KEY_CREATE_OUTPUT}" | jq -r ".publicKey")
MONGODB_ATLAS_PRIVATE_KEY=$(echo "${MONGODB_ATLAS_API_KEY_CREATE_OUTPUT}" | jq -r ".privateKey")


# Ensure Github secrets are set
echo "Creating github secrets"
function storeSecret {
    SECRET_NAME="${1}"
    SECRET_VALUE="${!SECRET_NAME}"
    if [ -z "${SECRET_VALUE}" ]; then
        echo "Missing environment variable '${SECRET_NAME}', please configure one in your '.env.local' file."
        echo "${SECRET_VALUE}"
        exit 1            
    fi
    
    echo "${SECRET_VALUE}" | gh secret set "${SECRET_NAME}" --app actions
    echo "${SECRET_NAME}=\"${SECRET_VALUE}\"" >> .env.deploy.local
}
cat /dev/null > .env.deploy.local
storeSecret GOOGLE_WORKLOAD_IDENTITY_PROVIDER
storeSecret GOOGLE_SERVICE_ACCOUNT_EMAIL
storeSecret GOOGLE_REGION
storeSecret GOOGLE_PROJECT_ID

storeSecret MONGODB_ATLAS_PRIVATE_KEY
storeSecret MONGODB_ATLAS_PUBLIC_KEY
storeSecret MONGODB_ATLAS_PROJECT_ID

storeSecret GOOGLE_OAUTH_CLIENT_ID
storeSecret GOOGLE_OAUTH_CLIENT_SECRET
storeSecret CLOUDFLARE_API_TOKEN
storeSecret CLOUDFLARE_ACCOUNT_ID
storeSecret SENTRY_DSN