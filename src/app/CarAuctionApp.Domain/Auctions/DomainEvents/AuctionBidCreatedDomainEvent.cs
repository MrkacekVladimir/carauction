using CarAuctionApp.SharedKernel.Domain;

namespace CarAuctionApp.Domain.Auctions.DomainEvents;

public record AuctionBidCreatedDomainEvent(Guid auctionId, decimal amount): IDomainEvent;
