# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV ASPNETCORE_ENVIROMENT=Development
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["BankAPI/BankAPI.csproj", "BankAPI/"]
RUN dotnet restore "BankAPI/BankAPI.csproj"

# Copy everything else and build the application
COPY . .
WORKDIR "/src/BankAPI"
RUN dotnet build "BankAPI.csproj" -c Release -o /app/build

# Publish image
FROM build AS publish
RUN dotnet publish "BankAPI.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankAPI.dll"]