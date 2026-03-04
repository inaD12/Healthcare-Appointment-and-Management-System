FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5100

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Appointments/Appointments.API/", "Appointments/Appointments.API/"]
COPY ["src/Appointments/Appointments.Application/", "Appointments/Appointments.Application/"]
COPY ["src/Appointments/Appointments.Domain/", "Appointments/Appointments.Domain/"]
COPY ["src/Appointments/Appointments.Infrastructure/", "Appointments/Appointments.Infrastructure/"]
COPY ["src/Shared/Shared.API/", "Shared/Shared.API/"]
COPY ["src/Shared/Shared.Application/", "Shared/Shared.Application/"]
COPY ["src/Shared/Shared.Domain/", "Shared/Shared.Domain/"]
COPY ["src/Shared/Shared.Infrastructure/", "Shared/Shared.Infrastructure/"]
COPY ["src/Shared/Shared/", "Shared/Shared/"]


WORKDIR "/src/Appointments/Appointments.API"

RUN dotnet restore "Appointments.API.csproj"

RUN dotnet build "Appointments.API.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Appointments/Appointments.API"
RUN dotnet publish "Appointments.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Appointments.API.dll"]