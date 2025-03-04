using CarAuctionApp.Domain.Auctions.DomainEvents;
using CarAuctionApp.Domain.Auctions.ValueObjects;
using CarAuctionApp.SharedKernel;
using CarAuctionApp.Domain.Users.Entities;
using CarAuctionApp.SharedKernel.Domain;

namespace CarAuctionApp.Domain.Auctions.Entities;

public class Auction : AggregateRoot
{
#pragma warning disable CS8618 
    // EF Core required
    private Auction()
    {
    }
#pragma warning restore CS8618 

    protected Auction(User user, string title, AuctionDate auctionDate)
    {
        Id = Guid.NewGuid();
        OwnerId = user.Id;
        Owner = user;
        Title = title;
        Date = auctionDate;

        _domainEvents.Add(new AuctionCreatedDomainEvent(Id, title));
    }

    public static Result<Auction?> Create(User user, string title, AuctionDate auctionDate)
    {
        var auction = new Auction(user, title, auctionDate);

        return Result<Auction?>.Success(auction);
    }


    public Guid Id { get; private set; }
    public Guid OwnerId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; } = "";
    public AuctionDate Date { get; private set; }
    public User Owner { get; private set; }

    private List<AuctionBid> _bids = new List<AuctionBid>();
    public IReadOnlyCollection<AuctionBid> Bids => _bids.AsReadOnly();

    public Result<AuctionBid?> AddBid(User user, BidAmount amount)
    {
        if (user.Id == Owner.Id)
        {
            //TODO: Have constants based on the error codes
            return Result<AuctionBid?>.Failure(new Error("InvalidBidder", $"Cannot create a bid as an owner of the auction."));
        }

        AuctionBid? maxBid = Bids.MaxBy(b => b.Amount.Value);
        if (maxBid is not null && maxBid.Amount.Value >= amount.Value)
        {
            //TODO: Have constants based on the error codes
            return Result<AuctionBid?>.Failure(new Error("InvalidBidAmount", $"Cannot create a bid with the same or lower amount. Current: {maxBid.Amount}, provided: {amount}."));
        }

        AuctionBid bid = new AuctionBid(this, user, amount);
        var updateResult = Date.UpdateLastBidOn(DateTime.UtcNow);
        if (!updateResult.IsSuccess)
        {
            return Result<AuctionBid?>.Failure(updateResult.Error);

        }

        _bids.Add(bid);
        _domainEvents.Add(new AuctionBidCreatedDomainEvent(bid.Id, Id, amount.Value));

        TimeSpan timeLeft = Date.EndsOn - Date.LastBidOn!.Value;
        if (timeLeft.TotalSeconds < 60)
        {
            Date.ExtendEndsOn(TimeSpan.FromMinutes(1));
            _domainEvents.Add(new AuctionEndsOnExtendedDomainEvent(Id, Date.EndsOn));
        }

        return Result<AuctionBid?>.Success(bid);
    }
}
