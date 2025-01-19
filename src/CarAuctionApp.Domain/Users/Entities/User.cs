using CarAuctionApp.Domain.Users.DomainEvents;

namespace CarAuctionApp.Domain.Users.Entities;

public class User: AggregateRoot
{
    protected User()
    {
        Username = null!;
    }

    public User(string username)
    {
        Id = Guid.NewGuid();
        Username = username;

        _domainEvents.Add(new UserCreated(Id, username));
    }

    public Guid Id { get; private set; }
    public string Username { get; private set; }
}
