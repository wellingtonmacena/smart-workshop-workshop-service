# ============================================================================
# Multi-stage build for Workshop Service API
# ============================================================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY SmartWorkshop.Workshop.slnx .
COPY SmartWorkshop.Workshop.Api/*.csproj ./SmartWorkshop.Workshop.Api/
COPY SmartWorkshop.Workshop.Application/*.csproj ./SmartWorkshop.Workshop.Application/
COPY SmartWorkshop.Workshop.Domain/*.csproj ./SmartWorkshop.Workshop.Domain/
COPY SmartWorkshop.Workshop.Infrastructure/*.csproj ./SmartWorkshop.Workshop.Infrastructure/

# Restore dependencies
RUN dotnet restore SmartWorkshop.Workshop.Api/SmartWorkshop.Workshop.Api.csproj

# Copy all source files
COPY . .

# Build and publish
WORKDIR /src/SmartWorkshop.Workshop.Api
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ============================================================================
# Runtime stage
# ============================================================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published files
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Healthcheck
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Set entrypoint
ENTRYPOINT ["dotnet", "SmartWorkshop.Workshop.Api.dll"]
