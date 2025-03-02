namespace CarAuctionApp.Contracts.IntegrationEvents.Auctions;

public record AuctionBidCreatedIntegrationEvent(Guid BidId, Guid AuctionId, decimal Amount) : IIntegrationEvent;
