using CarAuctionApp.Domain.Auctions.ValueObjects;
using CarAuctionApp.Domain.Users.Entities;
using CarAuctionApp.SharedKernel.Domain;

namespace CarAuctionApp.Domain.Auctions.Entities;

public class AuctionBid : EntityBase
{
    protected AuctionBid()
    {
        Amount = null!; 
        Auction = null!;
        User = null!;
    }

    public AuctionBid(Auction auction, User user, BidAmount amount)
    {
        Id = Guid.NewGuid();

        AuctionId = auction.Id;
        Auction = auction;

        UserId = user.Id;
        User = user;

        Amount = amount;
    }

    public Guid Id { get; private set; }

    public Guid AuctionId { get; private set; }
    public Auction Auction { get; private set; } 

    public Guid UserId { get; private set; }
    public User User { get; private set; }

    public BidAmount Amount { get; private set; }

}
