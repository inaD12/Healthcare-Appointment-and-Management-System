FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5100
#EXPOSE 5101

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Users/Users.API/", "Users/Users.API/"]
COPY ["src/Users/Users.Application/", "Users/Users.Application/"]
COPY ["src/Users/Users.Domain/", "Users/Users.Domain/"]
COPY ["src/Users/Users.Infrastructure/", "Users/Users.Infrastructure/"]
COPY ["src/Shared/Shared.API/", "Shared/Shared.API/"]
COPY ["src/Shared/Shared.Application/", "Shared/Shared.Application/"]
COPY ["src/Shared/Shared.Domain/", "Shared/Shared.Domain/"]
COPY ["src/Shared/Shared.Infrastructure/", "Shared/Shared.Infrastructure/"]

WORKDIR "/src/Users/Users.API"

RUN dotnet restore "Users.API.csproj"

RUN dotnet build "Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Users/Users.API"
RUN dotnet publish "Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.API.dll"]