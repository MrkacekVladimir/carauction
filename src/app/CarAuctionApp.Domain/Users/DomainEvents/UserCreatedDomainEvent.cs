using CarAuctionApp.SharedKernel.Domain;

namespace CarAuctionApp.Domain.Users.DomainEvents;

public record UserCreatedDomainEvent(Guid Id, string Username) : IDomainEvent;
