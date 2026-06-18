#!/usr/bin/env bash
set -e

echo "Adding System.IdentityModel.Tokens.Jwt to API project..."

dotnet add BMS-Logistics.API/BMS-Logistics.API.csproj package System.IdentityModel.Tokens.Jwt

echo "Done."
