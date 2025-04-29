# Use the official ASP.NET 8.0 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5137

# Use the official .NET 8.0 SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project files
COPY ["MEDIT.csproj", "./"]
RUN dotnet restore "./MEDIT.csproj"

# Copy the rest of the application
COPY . .

# Publish the application
RUN dotnet publish "MEDIT.csproj" -c Release -o /app/publish

# Build the final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "MEDIT.dll"]
