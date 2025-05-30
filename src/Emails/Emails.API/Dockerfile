FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5300
EXPOSE 5301

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Emails/Emails.API/", "Emails.API/"]
COPY ["src/Emails/Emails.Application/", "Emails.Application/"]
COPY ["src/Emails/Emails.Domain/", "Emails.Domain/"]
COPY ["src/Emails/Emails.Infrastructure/", "Emails.Infrastructure/"]
COPY ["src/Shared/Shared.API/", "Shared.API/"]
COPY ["src/Shared/Shared.Application/", "Shared.Application/"]
COPY ["src/Shared/Shared.Domain/", "Shared.Domain/"]
COPY ["src/Shared/Shared.Infrastructure/", "Shared.Infrastructure/"]

WORKDIR "/src/Emails.API"

RUN dotnet restore "Emails.API.csproj"

RUN dotnet build "Emails.API.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Emails.API"
RUN dotnet publish "Emails.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Emails.API.dll"]