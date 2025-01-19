namespace CarAuctionApp.Domain.Users.DomainEvents;

public record UserCreated(Guid Id, string Username) : IDomainEvent;
