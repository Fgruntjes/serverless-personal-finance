#!/usr/bin/env bash

set -e

cd "$(dirname "$(realpath "$0")")/.."

function isFunction() {
    PROJECT_NAME=$1
    [[ ! "${PROJECT_NAME}" =~ .Tests$ ]] && [[ "${PROJECT_NAME}" =~ ^App.Function ]]
}

function isDocker() {
    PROJECT_NAME=$1
    [[ -f "${PROJECT_DIRECTORY}/Dockerfile" ]]
}

function isTest() {
    PROJECT_NAME=$1
    [[ "${PROJECT_NAME}" =~ \.Tests$ && "${PROJECT_NAME}" != "App.Lib.Tests" ]]
}

function isTypescript() {
    PROJECT_NAME=$1
    [[ -f "${PROJECT_DIRECTORY}/tsconfig.json" ]]
}


RESULT_PROJECTS=()
PROJECT_DIRECTORIES=( $(find . -maxdepth 1 -type d) )
for PROJECT_DIRECTORY in "${PROJECT_DIRECTORIES[@]}"
do
    PROJECT_NAME=$(basename "${PROJECT_DIRECTORY}")
    
    case $1 in
        functions)
            if isFunction "${PROJECT_NAME}"; then
                RESULT_PROJECTS+=($PROJECT_NAME)
            fi
            ;;
        docker)
            if isDocker "${PROJECT_NAME}"; then
                RESULT_PROJECTS+=($PROJECT_NAME)
            fi
            ;;
        tests)
            if isTest "${PROJECT_NAME}"; then
                RESULT_PROJECTS+=($PROJECT_NAME)
            fi
            ;;
        typescript)
            if isTypescript "${PROJECT_NAME}"; then
                RESULT_PROJECTS+=($PROJECT_NAME)
            fi
            ;;
        *)
            echo "Unknown project type ${1}"
            echo "Usage: project_matrix.sh {functions|tests|typescript|docker} [--json]"
            exit 1
            ;;
    esac
done

if [[ "${2}" == "--json" ]]; then
    jq --compact-output --null-input '$ARGS.positional' --args -- "${RESULT_PROJECTS[@]}"
else
    for PROJECT in "${RESULT_PROJECTS[@]}"
    do
         echo "${PROJECT}"
    done
fi