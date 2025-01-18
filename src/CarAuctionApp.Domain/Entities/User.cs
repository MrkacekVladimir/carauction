namespace CarAuctionApp.Domain.Entities;

public class User
{
    protected User()
    {
        Username = null!;
    }

    public User(string username)
    {
        Username = username;
    }

    public Guid Id { get; private set; }
    public string Username { get; private set; }
}
