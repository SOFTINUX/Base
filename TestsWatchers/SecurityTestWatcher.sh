#!/bin/bash
dotnet watch --project ../src/Testing/Unit/SoftinuxBase.SecurityTests/SoftinuxBase.SecurityTests.csproj test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./lcov.info