FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 6000
EXPOSE 6001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CarAuctionApp.Reporting.WebApi/CarAuctionApp.Reporting.WebApi.csproj", "CarAuctionApp.Reporting.WebApi/"]
RUN dotnet restore "./CarAuctionApp.Reporting.WebApi/CarAuctionApp.Reporting.WebApi.csproj"
COPY . .
WORKDIR "/src/CarAuctionApp.Reporting.WebApi"
RUN dotnet build "./CarAuctionApp.Reporting.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarAuctionApp.Reporting.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarAuctionApp.Reporting.WebApi.dll"]