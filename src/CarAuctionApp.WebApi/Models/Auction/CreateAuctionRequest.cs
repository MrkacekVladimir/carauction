using System.Text.Json.Serialization;

namespace CarAuctionApp.WebApi.Models.Auction
{
    public record CreateAuctionRequest(
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("startsOn")] DateTime StartsOn,
        [property: JsonPropertyName("endsOn")] DateTime EndsOn
        );
}
