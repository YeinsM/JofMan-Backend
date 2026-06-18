#!/usr/bin/env bash
set -e

# Use this script to align MediatR to 14.1.0 and add BCrypt and System.IdentityModel.Tokens.Jwt

echo "Adding NuGet packages (aligned versions)..."

dotnet add BMS-Logistics.API/BMS-Logistics.API.csproj package BCrypt.Net-Next
dotnet add BMS-Logistics.API/BMS-Logistics.API.csproj package MediatR --version 11.1.0
dotnet add BMS-Logistics.API/BMS-Logistics.API.csproj package MediatR.Extensions.Microsoft.DependencyInjection --version 11.1.0
dotnet add BMS-Logistics.API/BMS-Logistics.API.csproj package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add BMS-Logistics.API/BMS-Logistics.API.csproj package System.IdentityModel.Tokens.Jwt
dotnet add BMS-Logistics.API/BMS-Logistics.API.csproj package Swashbuckle.AspNetCore

dotnet add BMS-Logistics.Application/BMS-Logistics.Application.csproj package MediatR --version 11.1.0
dotnet add BMS-Logistics.Application/BMS-Logistics.Application.csproj package BCrypt.Net-Next
dotnet add BMS-Logistics.Infrastructure/BMS-Logistics.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add BMS-Logistics.Infrastructure/BMS-Logistics.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer

# Ensure project reference
dotnet add BMS-Logistics.API/BMS-Logistics.API.csproj reference BMS-Logistics.Application/BMS-Logistics.Application.csproj

echo "Restoring packages..."
dotnet restore

echo "Done."
