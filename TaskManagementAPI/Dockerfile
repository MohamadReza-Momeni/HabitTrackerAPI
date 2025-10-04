# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# ✅ Copy using JSON array syntax to handle spaces
COPY ["TaskManagementAPI.csproj", "TaskManagementAPI.csproj"]

# Restore
RUN dotnet restore "TaskManagementAPI.csproj"

# Copy rest of the source code
COPY . .

# Publish
RUN dotnet publish "TaskManagementAPI.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5250
EXPOSE 7175
ENTRYPOINT ["dotnet", "TaskManagementAPI.dll"]
