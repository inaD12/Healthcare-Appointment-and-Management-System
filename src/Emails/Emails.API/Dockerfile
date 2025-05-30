FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5300
EXPOSE 5301

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Emails.API/Emails.API.csproj", "Emails.API/"]
COPY ["../Shared/Shared.API/Shared.API.csproj", "Shared.API/"]
COPY ["../Shared/Shared.Domain/Shared.Domain.csproj", "Shared.Domain/"]
COPY ["Emails.Application/Emails.Application.csproj", "Emails.Application/"]
COPY ["../Shared/Shared.Application/Shared.Application.csproj", "Shared.Application/"]
COPY ["../Shared/Shared.Infrastructure/Shared.Infrastructure.csproj", "Shared.Infrastructure/"]
RUN dotnet restore "Emails.API/Emails.API.csproj"
COPY . .
WORKDIR "/src/Emails.API"
RUN dotnet build "Emails.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Emails.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Emails.API.dll"]