#!/bin/bash
set -e

dotnet nuget push ${TRAVIS_BUILD_DIR}/artifacts/ReqToCurl.*.nupkg --api-key ${NUGET_API_KEY} -s https://api.nuget.org/v3/index.json