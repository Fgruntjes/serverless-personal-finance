#!/usr/bin/env bash

set -e

cd "$(dirname "$(realpath "$0")")/.."

function isPrEnvironment {
  ENVIRONMENT_NAME=$1
  [[ "${ENVIRONMENT_NAME}" =~ ^(pr|it) ]]
}

MAX_ENVIRONMENT_CREATED_AT=$(date -d "$2" +%s)

RESULT_ENVIRONMENTS=()
ENVIRONMENT_LIST=($(gcloud run services list --format=json | jq -c '.[].metadata'))
for ENVIRONMENT_INFO in "${ENVIRONMENT_LIST[@]}"
do
    ENVIRONMENT_CREATED_AT=$(date -d "$(echo "${ENVIRONMENT_INFO}" | jq -r '.creationTimestamp')" +%s)
    ENVIRONMENT_NAME=$(echo "${ENVIRONMENT_INFO}" | jq -r '.labels.environment')
    
    if [ "${MAX_ENVIRONMENT_CREATED_AT}" -lt "${ENVIRONMENT_CREATED_AT}" ]; then
        continue 
    fi
            
    case $1 in
        pr)
            if isPrEnvironment "${ENVIRONMENT_NAME}"; then
                RESULT_ENVIRONMENTS+=($ENVIRONMENT_NAME)
            fi
            ;;
        release)
            if ! isPrEnvironment "${ENVIRONMENT_NAME}"; then
                RESULT_ENVIRONMENTS+=($ENVIRONMENT_NAME)
            fi
            ;;
        *)
            echo "Unknown environment type ${1}"
            echo "Usage: environment_matrix.sh {pr|release} <min_age> [--json]"
            echo "  example: environment_matrix.sh pr '+4 hours'"
            exit 1
            ;;
    esac
done

if [[ "${3}" == "--json" ]]; then
    jq --compact-output --null-input '$ARGS.positional' --args -- "${RESULT_ENVIRONMENTS[@]}"
else
    for ENVIRONMENT in "${RESULT_ENVIRONMENTS[@]}"
    do
         echo "${ENVIRONMENT}"
    done
fi