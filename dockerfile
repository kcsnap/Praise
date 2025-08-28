# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR "/source/praise.api.source"
COPY ..
RUN dotnet restore "./praise.api/praise.api.csproj" --disable-parallel
RUN dotnet publish "./praise.api/praise.api.csproj" -c Release -o /app --no-restore

# Serve stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS serve
WORKDIR /app/praise.api
COPY --from=build /app .

EXPOSE 5000

ENTRYPOINT ["dotnet", "praise.api.dll"]