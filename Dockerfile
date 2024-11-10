# Use the .NET SDK as the build environment
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Install Node.js and npm
RUN apt-get update && \
    apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get install -y nodejs

COPY . .

# build Tailwind CSS
WORKDIR /src/NoBullshitTimer/Client
RUN npm install
RUN npx tailwindcss -i ./Styles/app.css -o ./wwwroot/css/app.css --minify


WORKDIR /src
RUN dotnet restore NoBullshitTimer.sln
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NoBullshitTimer.Server.dll"]