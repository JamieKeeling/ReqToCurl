#!/bin/bash
set -e

dotnet pack ${TRAVIS_BUILD_DIR}/src/ReqToCurl/ReqToCurl.csproj -c Release -o ${TRAVIS_BUILD_DIR}/artifacts --no-build -p:PackageVersion=${TRAVIS_TAG}
dotnet nuget push ${TRAVIS_BUILD_DIR}/artifacts/ReqToCurl.*.nupkg --api-key ${NUGET_API_KEY} -s https://api.nuget.org/v3/index.json