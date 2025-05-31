FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Gateway/Gateway.API/", "Gateway/Gateway.API/"]
COPY ["src/Shared/Shared.API/", "Shared/Shared.API/"]
COPY ["src/Shared/Shared.Application/", "Shared/Shared.Application/"]
COPY ["src/Shared/Shared.Domain/", "Shared/Shared.Domain/"]
COPY ["src/Shared/Shared.Infrastructure/", "Shared/Shared.Infrastructure/"]

WORKDIR "/src/Gateway.API"

RUN dotnet restore "Gateway.API.csproj"

RUN dotnet build "Gateway.API.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Gateway.API"
RUN dotnet publish "Gateway.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.API.dll"]