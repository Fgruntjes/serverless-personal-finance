#!/usr/bin/env bash

cd "$(dirname "$(realpath "$0")")";

set -e
set -x

API_PROJECTS=( $(../.github/project_matrix.sh functions) )
 
# Clear old generated code
rm -Rf src/generated/*/*

# Build dotnet projects and restore tools
(cd ../ && dotnet build && dotnet tool restore)
  
for PROJECT in "${API_PROJECTS[@]}"
do
    (
        cd ../
        export DOTNET_ENVIRONMENT=SwaggerBuild
        dotnet swagger tofile \
            --output "${PROJECT}/swagger.json" \
            "${PROJECT}/bin/Debug/net7.0/${PROJECT}.dll" \
            v1        
    )
        
	  ./node_modules/.bin/openapi \
        --input "../${PROJECT}/swagger.json" \
        --output "src/generated/${PROJECT}"
done
