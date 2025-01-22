using System.Text.Json.Serialization;

namespace CarAuctionApp.WebApi.Models.Auction;

public record GetAuctionsResponse([property: JsonPropertyName("auctions")] List<AuctionListItemDto> Auctions);
