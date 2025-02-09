namespace CarAuctionApp.Reporting.Data.Entities;

public class AuctionBid
{
    public Guid BidId { get; init; }
    public Guid AuctionId { get; init; }
    public Guid UserId { get; init; }
    public decimal Amount { get; init; }
}
