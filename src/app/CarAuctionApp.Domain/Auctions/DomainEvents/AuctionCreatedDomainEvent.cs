using CarAuctionApp.SharedKernel.Domain;

namespace CarAuctionApp.Domain.Auctions.DomainEvents;

public record AuctionCreatedDomainEvent(Guid AuctionId, string Title) : IDomainEvent;
