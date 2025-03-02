namespace CarAuctionApp.WebApi.Models.Auctions
{
    public record CreateAuctionRequest(string Title, DateTime StartsOn, DateTime EndsOn);
}
