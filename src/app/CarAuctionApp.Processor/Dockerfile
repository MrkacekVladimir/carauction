FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5020
EXPOSE 5021


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["app/CarAuctionApp.Processor/CarAuctionApp.Processor.csproj", "app/CarAuctionApp.Processor/"]
RUN dotnet restore "./app/CarAuctionApp.Processor/CarAuctionApp.Processor.csproj"
COPY . .
WORKDIR "/src/app/CarAuctionApp.Processor"
RUN dotnet build "./CarAuctionApp.Processor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarAuctionApp.Processor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarAuctionApp.Processor.dll"]