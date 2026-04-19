# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the csproj and restore dependencies
COPY ["HabitTracker.API.csproj", "./"]
RUN dotnet restore "HabitTracker.API.csproj"

# Copy the rest of the source code and build the release
COPY . .
RUN dotnet publish "HabitTracker.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Render assigns a dynamic PORT via environment variables. 
# ASP.NET Core 8.0+ by default uses 8080.
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "HabitTracker.API.dll"]
