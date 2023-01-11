#!/usr/bin/env bash

cd "$(dirname "$(realpath "$0")")";

set +x

FUNCTION_INTEGRATION_YNAB_URL="${FUNCTION_INTEGRATION_YNAB_URL:-http://localhost:5072}"

rm -Rf src/generated/*/*
./node_modules/.bin/openapi \
  --input "${FUNCTION_INTEGRATION_YNAB_URL}/swagger/v1/swagger.json" \
  --output src/generated/functionIntegrationYnab
