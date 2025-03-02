using CarAuctionApp.SharedKernel.Domain;

namespace CarAuctionApp.Domain.Auctions.DomainEvents;

public record AuctionBidCreatedDomainEvent(Guid BidId, Guid AuctionId, decimal Amount) : IDomainEvent;
