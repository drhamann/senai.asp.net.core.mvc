#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Authentication.Service/Authentication.Service.csproj", "Authentication.Service/"]
COPY ["Authentication.Application/Authentication.Application.csproj", "Authentication.Application/"]
COPY ["Authentication.Domain/Authentication.Domain.csproj", "Authentication.Domain/"]
COPY ["Authentication.Infrastructure/Authentication.Infrastructure.csproj", "Authentication.Infrastructure/"]
RUN dotnet restore "./Authentication.Service/./Authentication.Service.csproj"
COPY . .
WORKDIR "/src/Authentication.Service"
RUN dotnet build "./Authentication.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Authentication.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authentication.Service.dll"]