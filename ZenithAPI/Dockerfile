# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore
COPY ZenithAPI/ZenithAPI.csproj ZenithAPI/
RUN dotnet restore ZenithAPI/ZenithAPI.csproj

# Copy all source files and publish
COPY . .
WORKDIR /src/ZenithAPI
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose standard web port
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ZenithAPI.dll"]