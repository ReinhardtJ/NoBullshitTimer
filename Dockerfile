FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy solution file
COPY NoBullshitTimer.sln .

# Copy project files
COPY NoBullshitTimer/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p NoBullshitTimer/${file%.*}/ && mv $file NoBullshitTimer/${file%.*}/; done

# Restore as distinct layers
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet build -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NoBullshitTimer.Server.dll"]
