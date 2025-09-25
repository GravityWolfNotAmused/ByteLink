# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY . .
RUN ls -la
RUN dotnet restore ./ByteLink.MigrateCommand/ByteLink.MigrateCommand.csproj
RUN dotnet publish ./ByteLink.MigrateCommand/ByteLink.MigrateCommand.csproj -c Release -o /app/publish

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# Set the entrypoint to run your console application (adjust DLL name if different)
ENTRYPOINT ["dotnet", "ByteLink.MigrateCommand.dll"]
