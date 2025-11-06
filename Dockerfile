FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["UMessenger.sln", "./"]
COPY ["src/Messenger.API/Messenger.API.csproj", "./src/Messenger.API/"]
COPY ["src/Messenger.Application/Messenger.Application.csproj", "./src/Messenger.Application/"]
COPY ["src/Messenger.Domain/Messenger.Domain.csproj", "./src/Messenger.Domain/"]
COPY ["src/Messenger.Infrastructure/Messenger.Infrastructure.csproj", "./src/Messenger.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "UMessenger.sln"

# Copy the rest of the source code
COPY . .

# Build and publish
WORKDIR "/src/src/Messenger.API"
RUN dotnet publish "Messenger.API.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["dotnet", "Messenger.API.dll"]