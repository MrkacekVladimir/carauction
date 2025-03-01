using CarAuctionApp.SharedKernel.Domain;

namespace CarAuctionApp.Domain.Auctions.DomainEvents;

public record AuctionBidCreatedDomainEvent(Guid AuctionId, Guid AuctionBidId, decimal Amount): IDomainEvent;
