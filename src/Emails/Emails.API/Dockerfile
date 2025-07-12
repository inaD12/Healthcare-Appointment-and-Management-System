# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5300

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Emails/Emails.API/Emails.API.csproj", "Emails/Emails.API/"]
COPY ["src/Shared/Shared.API/Shared.API.csproj", "Shared/Shared.API/"]
COPY ["src/Shared/Shared.Domain/Shared.Domain.csproj", "Shared/Shared.Domain/"]
COPY ["src/Emails/Emails.Application/Emails.Application.csproj", "Emails/Emails.Application/"]
COPY ["src/Shared/Shared.Application/Shared.Application.csproj", "Shared/Shared.Application/"]
COPY ["src/Shared/Shared.Infrastructure/Shared.Infrastructure.csproj", "Shared/Shared.Infrastructure/"]
RUN dotnet restore "./Emails/Emails.API/Emails.API.csproj"
COPY . .
WORKDIR "/src/src/Emails/Emails.API"
RUN dotnet build "Emails.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Emails.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Emails.API.dll"]
