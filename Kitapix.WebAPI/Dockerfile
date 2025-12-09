# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Kitapix.WebAPI/Kitapix.WebAPI.csproj", "Kitapix.WebAPI/"]
COPY ["Kitapix.Application/Kitapix.Application.csproj", "Kitapix.Application/"]
COPY ["Kitapix.Domain/Kitapix.Domain.csproj", "Kitapix.Domain/"]
COPY ["Kitapix.Infrastructure/Kitapix.Infrastructure.csproj", "Kitapix.Infrastructure/"]
RUN dotnet restore "./Kitapix.WebAPI/Kitapix.WebAPI.csproj"
COPY . .
WORKDIR "/src/Kitapix.WebAPI"
RUN dotnet build "./Kitapix.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Kitapix.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kitapix.WebAPI.dll"]