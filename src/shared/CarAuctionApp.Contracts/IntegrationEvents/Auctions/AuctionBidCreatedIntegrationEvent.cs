namespace CarAuctionApp.Contracts.IntegrationEvents.Auctions;
public record AuctionBidCreatedIntegrationEvent(Guid AuctionId, Guid AuctionBidId, decimal Amount) : IIntegrationEvent;
