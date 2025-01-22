#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base stage with runtime dependencies
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5100
#EXPOSE 5101

# Create a non-root user and set permissions (Linux-specific)
RUN adduser --disabled-password appuser && chown -R appuser /app
USER appuser

# Build stage with SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Users/Users.API/Users.API.csproj", "src/Users/Users.API/"]
COPY ["src/Users/Users.Application/Users.Application.csproj", "src/Users/Users.Application/"]
COPY ["src/Users/Users.Domain/Users.Domain.csproj", "src/Users/Users.Domain/"]
COPY ["src/Users/Users.Infrastructure/Users.Infrastructure.csproj", "src/Users/Users.Infrastructure/"]
RUN dotnet restore "./src/Users/Users.API/Users.API.csproj"
COPY . .
WORKDIR "/src/src/Users/Users.API"
RUN dotnet build "./Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage with runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Ensure the non-root user can access all files in /app
USER appuser
ENTRYPOINT ["dotnet", "Users.API.dll"]