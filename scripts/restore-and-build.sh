#!/usr/bin/env bash
set -e

./scripts/add-dependencies-v2.sh

# Build solution
dotnet restore
dotnet build -v minimal

echo "Build complete"
