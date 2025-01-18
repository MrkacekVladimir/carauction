namespace CarAuctionApp.Domain.Entities;

public class AuctionBid
{
    protected AuctionBid()
    {

    }

    public AuctionBid(Auction auction, User user, decimal amount)
    {
        Auction = auction;
        User = user;
        Amount = amount;
    }

    public Guid Id { get; private set; }

    public Guid AuctionId { get; private set; }
    public Auction Auction { get; private set; } = null!;

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public decimal Amount { get; private set; }

}
