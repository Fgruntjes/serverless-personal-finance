#!/usr/bin/env bash

set -e
 
cd "$(dirname "$(realpath "$0")")";

set -a
test -f .env.deploy.local && source .env.deploy.local
test -f .env.local && source .env.local
set +a

# Install deploy dependencies
npm ci

ansible-playbook "${@}" \
    -e "APP_TAG=${APP_TAG}" \
    -e "APP_ENVIRONMENT=${APP_ENVIRONMENT}" \
    -e "APP_FRONTEND=${APP_FRONTEND}" \
    -e "SENTRY_DSN=${SENTRY_DSN}" \
    -e "GOOGLE_REGION=${GOOGLE_REGION}" \
    -e "GOOGLE_PROJECT_ID=${GOOGLE_PROJECT_ID}" \
    -e "MONGODB_ATLAS_PUBLIC_KEY=${MONGODB_ATLAS_PUBLIC_KEY}" \
    -e "MONGODB_ATLAS_PRIVATE_KEY=${MONGODB_ATLAS_PRIVATE_KEY}" \
    -e "MONGODB_ATLAS_PROJECT_ID=${MONGODB_ATLAS_PROJECT_ID}" \
    -e "CLOUDFLARE_API_TOKEN=${CLOUDFLARE_API_TOKEN}" \
    -e "CLOUDFLARE_ACCOUNT_ID=${CLOUDFLARE_ACCOUNT_ID}" \
    -e "AUTH0_DOMAIN=${AUTH0_DOMAIN}" \
    -e "AUTH0_CLIENT_ID=${AUTH0_CLIENT_ID}" \
    -e "AUTH0_CLIENT_SECRET=${AUTH0_CLIENT_SECRET}" \
    -e "YNAB_CLIENT_ID=${YNAB_CLIENT_ID}" \
    -e "YNAB_CLIENT_SECRET=${YNAB_CLIENT_SECRET}"
    