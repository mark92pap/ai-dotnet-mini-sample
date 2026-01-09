# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY *.sln ./
COPY AiMiniSample/AiMiniSample.csproj AiMiniSample/
COPY Apis/Clients/PetStoreClient/PetStoreClient.csproj Apis/Clients/PetStoreClient/
COPY Apis/Server/Server/GeneratedApi/GeneratedApi.csproj Apis/Server/Server/GeneratedApi/

# Restore dependencies
RUN dotnet restore

# Copy everything else
COPY . .

# Build the application
WORKDIR /src/AiMiniSample
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy published files
COPY --from=publish /app/publish .

# Create data directory for SQLite (as root before switching user)
RUN mkdir -p /app/data && chown -R app:app /app/data

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Switch to built-in non-root user
USER app

EXPOSE 8080

ENTRYPOINT ["dotnet", "AiMiniSample.dll"]
