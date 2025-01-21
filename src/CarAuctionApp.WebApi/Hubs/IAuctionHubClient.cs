namespace CarAuctionApp.WebApi.Hubs;

public interface IAuctionHubClient
{
    Task ReceiveBidUpdate(Guid auctionId, decimal bidAmount);
}

