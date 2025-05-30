FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5100
#EXPOSE 5101


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Users.API.csproj", "Users.API/"]
COPY ["Users.Application/Users.Application.csproj", "Users.Application/"]
COPY ["Users.Domain/Users.Domain.csproj", "Users.Domain/"]
COPY ["Users.Infrastructure/Users.Infrastructure.csproj", "Users.Infrastructure/"]
RUN dotnet restore "Users.API/Users.API.csproj"
COPY . .
WORKDIR "/src/Users.API"
RUN dotnet build "Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Users.API.dll"]