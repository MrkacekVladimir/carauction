namespace CarAuctionApp.Domain.Entities;

public class AuctionBid
{
    public Guid Id { get; private set; }
    public Guid AuctionId { get; private set; }
}
