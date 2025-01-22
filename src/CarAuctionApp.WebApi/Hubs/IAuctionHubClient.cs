namespace CarAuctionApp.WebApi.Hubs;

public interface IAuctionHubClient
{
    Task ReceiveBidUpdate(Guid auctionId, Guid bidId, decimal bidAmount, DateTime createdOn);
}

