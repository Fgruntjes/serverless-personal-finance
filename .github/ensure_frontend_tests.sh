#!/usr/bin/env bash

set +x

missing_tests () {
    find src -name '*.tsx' -not -name "*.test.tsx" | while read -r CODE_FILE; do
        TEST_FILE="$(echo "$CODE_FILE" | cut -d'.' -f1).test.tsx"
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