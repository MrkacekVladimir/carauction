using CarAuctionApp.SharedKernel.Domain;

namespace CarAuctionApp.Domain.Auctions.DomainEvents;

public record AuctionCreatedDomainEvent(Guid Id, string Title) : IDomainEvent;
