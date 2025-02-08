using System.Text.Json.Serialization;

namespace CarAuctionApp.WebApi.Models.Auction
{
    public record CreateBidRequest([property: JsonPropertyName("amount")] decimal Amount);
}
