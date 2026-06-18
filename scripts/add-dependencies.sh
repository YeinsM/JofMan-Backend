#!/usr/bin/env bash
set -e

# From solution root, run this script to add required NuGet packages and project reference

echo "Adding NuGet packages..."

dotnet add BMS-Logistics.API/BMS-Logistics.API.csproj package BCrypt.Net-Next
dotnet add BMS-Logistics.API/BMS-Logistics.API.csproj package MediatR --version 11.1.0
dotnet add BMS-Logistics.API/BMS-Logistics.API.csproj package MediatR.Extensions.Microsoft.DependencyInjection --version 11.1.0
dotnet add BMS-Logistics.API/BMS-Logistics.API.csproj package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add BMS-Logistics.API/BMS-Logistics.API.csproj package System.IdentityModel.Tokens.Jwt
dotnet add BMS-Logistics.Application/BMS-Logistics.Application.csproj package BCrypt.Net-Next

# Ensure application project has MediatR reference if handlers use MediatR types
dotnet add BMS-Logistics.Application/BMS-Logistics.Application.csproj package MediatR --version 11.1.0
dotnet add BMS-Logistics.Application/BMS-Logistics.Application.csproj package BCrypt.Net-Next

# Add project reference from API -> Application
dotnet add BMS-Logistics.API/BMS-Logistics.API.csproj reference BMS-Logistics.Application/BMS-Logistics.Application.csproj

echo "Restoring packages..."
dotnet restore

echo "Done."
