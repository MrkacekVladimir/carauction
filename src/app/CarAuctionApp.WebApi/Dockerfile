FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CarAuctionApp.WebApi/CarAuctionApp.WebApi.csproj", "CarAuctionApp.WebApi/"]
COPY ["CarAuctionApp.Application/CarAuctionApp.Application.csproj", "CarAuctionApp.Application/"]
COPY ["CarAuctionApp.Domain/CarAuctionApp.Domain.csproj", "CarAuctionApp.Domain/"]
COPY ["CarAuctionApp.Infrastructure/CarAuctionApp.Infrastructure.csproj", "CarAuctionApp.Infrastructure/"]
COPY ["CarAuctionApp.Persistence/CarAuctionApp.Persistence.csproj", "CarAuctionApp.Persistence/"]
RUN dotnet restore "./CarAuctionApp.WebApi/CarAuctionApp.WebApi.csproj"
COPY . .
WORKDIR "/src/CarAuctionApp.WebApi"
RUN dotnet build "./CarAuctionApp.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarAuctionApp.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarAuctionApp.WebApi.dll"]