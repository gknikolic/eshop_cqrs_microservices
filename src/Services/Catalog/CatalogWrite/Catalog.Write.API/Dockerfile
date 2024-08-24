# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Catalog/CatalogWrite/Catalog.Write.API/Catalog.Write.API.csproj", "Services/Catalog/CatalogWrite/Catalog.Write.API/"]
COPY ["Services/Catalog/CatalogWrite/Catalog.Write.Application/Catalog.Write.Application.csproj", "Services/Catalog/CatalogWrite/Catalog.Write.Application/"]
COPY ["BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "BuildingBlocks/BuildingBlocks.Messaging/"]
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
COPY ["Services/Catalog/CatalogWrite/Catalog.Write.Domain/Catalog.Write.Domain.csproj", "Services/Catalog/CatalogWrite/Catalog.Write.Domain/"]
COPY ["Services/Catalog/CatalogWrite/Catalog.Write.Infrastructure/Catalog.Write.Infrastructure.csproj", "Services/Catalog/CatalogWrite/Catalog.Write.Infrastructure/"]
RUN dotnet restore "./Services/Catalog/CatalogWrite/Catalog.Write.API/Catalog.Write.API.csproj"
COPY . .
WORKDIR "/src/Services/Catalog/CatalogWrite/Catalog.Write.API"
RUN dotnet build "./Catalog.Write.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Catalog.Write.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Write.API.dll"]