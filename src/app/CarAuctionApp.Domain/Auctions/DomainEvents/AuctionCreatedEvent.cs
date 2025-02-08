using CarAuctionApp.SharedKernel;

namespace CarAuctionApp.Domain.Auctions.DomainEvents;

public record AuctionCreatedEvent(Guid Id, string Title) : IDomainEvent;
