using CarAuctionApp.SharedKernel.Domain;

namespace CarAuctionApp.Domain.Auctions.DomainEvents;

public record AuctionEndsOnExtendedDomainEvent(Guid auctionId, DateTime endsOn): IDomainEvent;
