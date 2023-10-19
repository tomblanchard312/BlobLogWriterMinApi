#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
# Use the Azurite image as the base
FROM mcr.microsoft.com/azure-storage/azurite
# Set the working directory
WORKDIR /app

# Expose Azurite blob storage emulator ports
EXPOSE 10000
EXPOSE 10001

# Build and publish the .NET 6 application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BlobLogWriterMinApi/BlobLogWriterMinApi.csproj", "BlobLogWriterMinApi/"]
RUN dotnet restore "BlobLogWriterMinApi/BlobLogWriterMinApi.csproj"
COPY . .
WORKDIR "/src/BlobLogWriterMinApi"
RUN dotnet build "BlobLogWriterMinApi.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "BlobLogWriterMinApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlobLogWriterMinApi.dll"]
