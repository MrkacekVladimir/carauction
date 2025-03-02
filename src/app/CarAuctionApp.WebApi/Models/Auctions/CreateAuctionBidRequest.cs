namespace CarAuctionApp.WebApi.Models.Auctions;

public record CreateAuctionBidRequest(Guid AuctionId, decimal Amount);
