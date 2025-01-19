namespace CarAuctionApp.Domain.Auctions.ValueObjects;

public class BidAmount
{
    private BidAmount() { }
    public BidAmount(decimal value)
    {
        if(Value <= 0)
        {
            throw new ArgumentException("Bid amount must be greater than zero.");
        }

        Value = value;
    }

    public decimal Value { get; set; }
}
