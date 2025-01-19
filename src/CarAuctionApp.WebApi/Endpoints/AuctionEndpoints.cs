using CarAuctionApp.Domain;
using CarAuctionApp.Domain.Auctions.Entities;
using CarAuctionApp.Domain.Auctions.Repositories;
using CarAuctionApp.Domain.Auctions.ValueObjects;
using CarAuctionApp.Persistence;
using CarAuctionApp.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApp.WebApi.Endpoints;

internal record CreateAuctionModel(string Title);
internal record CreateBidModel(decimal Amount);

internal record AuctionBidUserDto(Guid Id, string Username);
internal record AuctionBidDto(Guid Id, decimal Amount, AuctionBidUserDto User);
internal record AuctionListItemDto(Guid Id, string Title, IEnumerable<AuctionBidDto> bids);

internal static class AuctionEndpoints
{
    public static void MapAuctionEndpoints(this WebApplication app)
    {
        var auctionsGroup = app.MapGroup("/auctions");

        auctionsGroup.MapGet("/", async (AuctionDbContext dbContext) =>
        {
            var auctions = await dbContext.Auctions.AsNoTracking()
            .Select(a => 
            new AuctionListItemDto(
                a.Id,
                a.Title,
                a.Bids.Select(b => new AuctionBidDto(
                    b.Id,
                    b.Amount.Value,
                    new AuctionBidUserDto(b.User.Id, b.User.Username)) 
                ) 
            )).ToListAsync();
            return Results.Json(auctions);

        }).WithName("GetAuctions");

        auctionsGroup.MapPost("/", async (CreateAuctionModel model, AuctionDbContext dbContext) =>
        {
            Auction auction = new Auction(model.Title);

            dbContext.Auctions.Add(auction);
            await dbContext.SaveChangesAsync();

            return Results.Json(auction);
        }).WithName("CreateAuction");

        auctionsGroup.MapGet("/{auctionId:Guid}", async (Guid auctionId, AuctionDbContext dbContext) =>
        {
            var auction = await dbContext.Auctions.FindAsync(auctionId);
            return Results.Json(auction);
        }).WithName("GetAuction");

        auctionsGroup.MapGet("/{auctionId:Guid}/bids", async (Guid auctionId, AuctionDbContext dbContext) =>
        {
            var bids = await dbContext.AuctionBids.Where(b => b.AuctionId == auctionId).ToListAsync();
            return Results.Json(bids);
        }).WithName("GetAuctionBids");

        auctionsGroup.MapPost("/{auctionId:Guid}/bids", async (
            Guid auctionId,
            CreateBidModel model,
            IAuctionRepository auctionRepository,
            IUnitOfWork unitOfWork,
            IHubContext<AuctionHub, IAuctionHubClient> hubContext,
            AuctionDbContext dbContext
            ) =>
        {
            //TODO: remove fake user 
            var user = await dbContext.Users.FirstAsync();

            var auction = await auctionRepository.GetById(auctionId);
            if (auction is null)
            {
                return Results.NotFound();
            }

            BidAmount amount = new(model.Amount);
            auction.AddBid(user, amount);

            await unitOfWork.SaveChangesAsync();

            await hubContext.Clients.Group(auctionId.ToString()).ReceiveBidUpdate(auctionId, model.Amount);

            //TODO: Return DTO to fix cyclic reference
            return Results.Json(auction, new System.Text.Json.JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            });
        }).WithName("CreateAuctionBid");

    }

}
