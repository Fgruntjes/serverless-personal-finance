#!/usr/bin/env bash

cd "$(dirname "$(realpath "$0")")";

set -e
set -x

API_PROJECTS=( 
  "App.Function.Integration.Ynab"
)
  
# Clear old generated code
rm -Rf src/generated/*/*

# Build dotnet projects
(cd ../ && dotnet build)
  
for PROJECT in "${API_PROJECTS[@]}"
do
    (
        cd ../
        dotnet swagger tofile \
            --output "${PROJECT}/swagger.json" \
            "${PROJECT}/bin/Debug/net7.0/${PROJECT}.dll" \
            v1        
    )
        
	  ./node_modules/.bin/openapi \
        --input "../${PROJECT}/swagger.json" \
        --output "src/generated/${PROJECT}"
done
