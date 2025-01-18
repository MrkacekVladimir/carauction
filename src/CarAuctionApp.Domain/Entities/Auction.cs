namespace CarAuctionApp.Domain.Entities;

public class Auction
{
    protected Auction()
    {
        Title = null!;
    }

    public Auction(string title)
    {
        Title = title;
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; }

    private List<AuctionBid> _bids = new List<AuctionBid>();
    public IReadOnlyCollection<AuctionBid> Bids => _bids.AsReadOnly();

    public AuctionBid AddBid(User user, decimal amount)
    {
        AuctionBid? maxBid = Bids.MaxBy(b => b.Amount);
        if (maxBid is not null && maxBid.Amount <= amount)
        {
            //TODO: Custom exception for business logic
            throw new Exception($"Cannot create a bid with the same or lower amount. Current: {maxBid.Amount}, provided: {amount}.");
        }

        AuctionBid bid = new AuctionBid(this, user, amount);
        _bids.Add(bid);
        return bid;
    }
}
