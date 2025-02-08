using System.Text.Json.Serialization;

namespace CarAuctionApp.WebApi.Models.Auction;

public record GetAuctionResponse([property: JsonPropertyName("auction")]AuctionDto Auction);
