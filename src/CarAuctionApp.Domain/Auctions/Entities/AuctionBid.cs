using CarAuctionApp.Domain.Auctions.ValueObjects;
using CarAuctionApp.Domain.Users.Entities;

namespace CarAuctionApp.Domain.Auctions.Entities;

public class AuctionBid: EntityBase    
{
    protected AuctionBid() { }

    public AuctionBid(Auction auction, User user, BidAmount amount)
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

    public BidAmount Amount { get; private set; }

}
