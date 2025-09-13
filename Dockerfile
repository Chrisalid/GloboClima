# Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/GloboClima.API/GloboClima.API.csproj", "src/GloboClima.API/"]
COPY ["src/GloboClima.Core/GloboClima.Core.csproj", "src/GloboClima.Core/"]
COPY ["src/GloboClima.Infrastructure/GloboClima.Infrastructure.csproj", "src/GloboClima.Infrastructure/"]
RUN dotnet restore "src/GloboClima.API/GloboClima.API.csproj"

# Copy source code and build
COPY . .
WORKDIR "/src/src/GloboClima.API"
RUN dotnet build "GloboClima.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "GloboClima.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy published app
COPY --from=publish /app/publish .

# Create non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "GloboClima.API.dll"]
