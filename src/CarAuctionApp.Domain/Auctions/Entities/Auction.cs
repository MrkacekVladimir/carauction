using CarAuctionApp.Domain.Auctions.DomainEvents;
using CarAuctionApp.Domain.Auctions.ValueObjects;
using CarAuctionApp.Domain.Users.Entities;

namespace CarAuctionApp.Domain.Auctions.Entities;

public class Auction: AggregateRoot
{
    protected Auction()
    {
        Title = null!;
    }

    public Auction(string title)
    {
        Id = Guid.NewGuid();
        Title = title;

        _domainEvents.Add(new AuctionCreatedEvent(Id, title));
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; }

    private List<AuctionBid> _bids = new List<AuctionBid>();
    public IReadOnlyCollection<AuctionBid> Bids => _bids.AsReadOnly();

    public AuctionBid AddBid(User user, BidAmount amount)
    {
        AuctionBid? maxBid = Bids.MaxBy(b => b.Amount.Value);
        if (maxBid is not null && maxBid.Amount.Value >= amount.Value)
        {
            //TODO: Custom exception for business logic
            throw new Exception($"Cannot create a bid with the same or lower amount. Current: {maxBid.Amount}, provided: {amount}.");
        }

        AuctionBid bid = new AuctionBid(this, user, amount);
        _bids.Add(bid);

        _domainEvents.Add(new AuctionBidCreatedEvent(Id, amount.Value));

        return bid;
    }
}
