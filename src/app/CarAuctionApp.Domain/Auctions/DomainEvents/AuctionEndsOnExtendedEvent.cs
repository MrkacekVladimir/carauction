using CarAuctionApp.SharedKernel;

namespace CarAuctionApp.Domain.Auctions.DomainEvents;

public record AuctionEndsOnExtendedEvent(Guid auctionId, DateTime endsOn): IDomainEvent;
