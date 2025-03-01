namespace CarAuctionApp.Contracts.IntegrationEvents.Auctions;

public record AuctionCreatedIntegrationEvent(Guid AuctionId, string Title) : IIntegrationEvent;
