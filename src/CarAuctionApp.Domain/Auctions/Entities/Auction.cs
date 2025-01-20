using CarAuctionApp.Domain.Auctions.DomainEvents;
using CarAuctionApp.Domain.Auctions.ValueObjects;
using CarAuctionApp.Domain.Shared;
using CarAuctionApp.Domain.Users.Entities;

namespace CarAuctionApp.Domain.Auctions.Entities;

public class Auction: AggregateRoot
{
    protected Auction()
    {
        Title = null!;
    }

    public Auction(string title, AuctionDate auctionDate)
    {
        Id = Guid.NewGuid();
        Title = title;
        Date = auctionDate;

        _domainEvents.Add(new AuctionCreatedEvent(Id, title));
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public AuctionDate Date { get; private set; }

    private List<AuctionBid> _bids = new List<AuctionBid>();
    public IReadOnlyCollection<AuctionBid> Bids => _bids.AsReadOnly();

    public Result<AuctionBid?> AddBid(User user, BidAmount amount)
    {
        AuctionBid? maxBid = Bids.MaxBy(b => b.Amount.Value);
        if (maxBid is not null && maxBid.Amount.Value >= amount.Value)
        {
            return Result<AuctionBid?>.Failure(new Error("AUCTION001", $"Cannot create a bid with the same or lower amount. Current: {maxBid.Amount}, provided: {amount}."));
        }

        AuctionBid bid = new AuctionBid(this, user, amount);
        Date.UpdateLastBidOn(DateTime.UtcNow);
        _bids.Add(bid);
        _domainEvents.Add(new AuctionBidCreatedEvent(Id, amount.Value));

        TimeSpan timeLeft = Date.EndsOn - Date.LastBidOn!.Value;
        if(timeLeft.TotalSeconds < 60)
        {
            Date.ExtendEndsOn(TimeSpan.FromMinutes(1));
            _domainEvents.Add(new AuctionEndsOnExtendedEvent(Id, Date.EndsOn));
        }

        return Result<AuctionBid?>.Success(bid);
    }
}
