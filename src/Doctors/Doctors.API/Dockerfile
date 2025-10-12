FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5300

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Doctors/Doctors.API/", "Doctors/Doctors.API/"]
COPY ["src/Doctors/Doctors.Application/", "Doctors/Doctors.Application/"]
COPY ["src/Doctors/Doctors.Domain/", "Doctors/Doctors.Domain/"]
COPY ["src/Doctors/Doctors.Infrastructure/", "Doctors/Doctors.Infrastructure/"]
COPY ["src/Shared/Shared.API/", "Shared/Shared.API/"]
COPY ["src/Shared/Shared.Application/", "Shared/Shared.Application/"]
COPY ["src/Shared/Shared.Domain/", "Shared/Shared.Domain/"]
COPY ["src/Shared/Shared.Infrastructure/", "Shared/Shared.Infrastructure/"]
COPY ["src/Shared/Shared/", "Shared/Shared/"]

WORKDIR "/src/Doctors/Doctors.API"

RUN dotnet restore "Doctors.API.csproj"

RUN dotnet build "Doctors.API.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Doctors/Doctors.API"
RUN dotnet publish "Doctors.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Doctors.API.dll"]