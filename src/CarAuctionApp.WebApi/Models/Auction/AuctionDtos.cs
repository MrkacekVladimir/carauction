using System.Text.Json.Serialization;

namespace CarAuctionApp.WebApi.Models.Auction;

public record AuctionBidUserDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("username")] string Username
    );

public record AuctionBidDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("amount")] decimal Amount,
    [property: JsonPropertyName("user")] AuctionBidUserDto User
    );
public record AuctionListItemDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("bids")] IEnumerable<AuctionBidDto> bids
    );
public record AuctionDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("startsOn")] DateTime StartsOn,
    [property: JsonPropertyName("endsOn")] DateTime EndsOn,
    [property: JsonPropertyName("bids")] IEnumerable<AuctionBidDto> Bids
    );
