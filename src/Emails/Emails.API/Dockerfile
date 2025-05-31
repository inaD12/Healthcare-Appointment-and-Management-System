FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5300
EXPOSE 5301

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Emails/Emails.API/", "Emails/Emails.API/"]
COPY ["src/Emails/Emails.Application/", "Emails/Emails.Application/"]
COPY ["src/Shared/Shared.API/", "Shared/Shared.API/"]
COPY ["src/Shared/Shared.Application/", "Shared/Shared.Application/"]
COPY ["src/Shared/Shared.Domain/", "Shared/Shared.Domain/"]
COPY ["src/Shared/Shared.Infrastructure/", "Shared/Shared.Infrastructure/"]
COPY ["src/Shared/Shared/", "Shared/Shared/"]

WORKDIR "/src/Emails/Emails.API"

RUN dotnet restore "Emails.API.csproj"

RUN dotnet build "Emails.API.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Emails/Emails.API"
RUN dotnet publish "Emails.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Emails.API.dll"]