using CarAuctionApp.SharedKernel;

namespace CarAuctionApp.Domain.Auctions.DomainEvents;

public record AuctionBidCreatedEvent(Guid auctionId, decimal amount): IDomainEvent;
