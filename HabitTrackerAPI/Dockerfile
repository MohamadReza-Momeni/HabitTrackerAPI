# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# ✅ Copy using JSON array syntax to handle spaces
COPY ["HabitTrackerAPI.csproj", "HabitTrackerAPI.csproj"]

# Restore
RUN dotnet restore "HabitTrackerAPI.csproj"

# Copy rest of the source code
COPY . .

# Publish
RUN dotnet publish "HabitTrackerAPI.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5250
EXPOSE 7175
ENTRYPOINT ["dotnet", "HabitTrackerAPI.dll"]
