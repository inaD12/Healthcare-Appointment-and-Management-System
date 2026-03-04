FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5600

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Patients/Patients.API/", "Patients/Patients.API/"]
COPY ["src/Patients/Patients.Application/", "Patients/Patients.Application/"]
COPY ["src/Patients/Patients.Domain/", "Patients/Patients.Domain/"]
COPY ["src/Patients/Patients.Infrastructure/", "Patients/Patients.Infrastructure/"]
COPY ["src/Shared/Shared.API/", "Shared/Shared.API/"]
COPY ["src/Shared/Shared.Application/", "Shared/Shared.Application/"]
COPY ["src/Shared/Shared.Domain/", "Shared/Shared.Domain/"]
COPY ["src/Shared/Shared.Infrastructure/", "Shared/Shared.Infrastructure/"]
COPY ["src/Shared/Shared/", "Shared/Shared/"]

WORKDIR "/src/Patients/Patients.API"

RUN dotnet restore "Patients.API.csproj"

RUN dotnet build "Patients.API.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Patients/Patients.API"
RUN dotnet publish "Patients.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Patients.API.dll"]