namespace CarAuctionApp.Domain.Auctions.ValueObjects;

public record BidAmount
{
    private BidAmount() { }
    public BidAmount(decimal value)
    {
        if(value <= 0)
        {
            throw new ArgumentException("Bid amount must be greater than zero.");
        }

        Value = value;
    }

    public decimal Value { get; private set; }
}
