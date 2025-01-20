namespace CarAuctionApp.Domain.Users.DomainEvents;

public record UserCreatedEvent(Guid Id, string Username) : IDomainEvent;
