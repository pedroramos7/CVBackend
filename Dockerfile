# Use the official .NET image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY CVBackend/*.csproj ./CVBackend/
RUN dotnet restore

# Copy everything else and build
COPY CVBackend/. ./CVBackend/
WORKDIR /app/CVBackend
RUN dotnet publish -c Release -o out

# Use the official ASP.NET image for a lean production environment
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/CVBackend/out ./
ENTRYPOINT ["dotnet", "CVBackend.dll"]
