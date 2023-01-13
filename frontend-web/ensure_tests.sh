#!/usr/bin/env bash

cd "$(dirname "$(realpath "$0")")";

missing_tests () {
    find src \
        -path src/generated -prune \
        -name '*.tsx' -or -name '*.ts' \
        -not -name "setupTests.ts" \
        -not -name "react-app-env.d.ts" \
        -not -name "*.test.tsx" -not -name "*.test.ts" \
        -not -name "*.mock.ts" -not -name "*.mock.tsx" \
            | while read -r CODE_FILE; do
        CODE_FILE_NAME=$(basename -- "$CODE_FILE")
        CODE_FILE_EXTENSION="${CODE_FILE_NAME##*.}"
        TEST_FILE="$(echo "$CODE_FILE" | cut -d'.' -f1).test.${CODE_FILE_EXTENSION}"
        if [ ! -f "$TEST_FILE" ]; then
            echo "Missing test for: ${CODE_FILE} (${TEST_FILE})"
        fi
    done
}

MISSING_TESTS=$(missing_tests)

echo "${MISSING_TESTS}"

if [ -z "$MISSING_TESTS" ]; then
    exit 0;
else
    exit 1;
fi