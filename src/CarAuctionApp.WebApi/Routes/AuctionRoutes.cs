using CarAuctionApp.Domain.Entities;
using CarAuctionApp.Infrastructure.Persistence;
using CarAuctionApp.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApp.WebApi.Routes;

record CreateAuctionModel(string Title);
record CreateBidModel(decimal Amount);

internal static class AuctionRoutes
{
    public static void MapAuctionRoutes(this WebApplication app)
    {
        var auctionsGroup = app.MapGroup("/auctions");

        auctionsGroup.MapGet("/", async (AuctionDbContext dbContext) =>
        {
            var auctions = await dbContext.Auctions.ToListAsync();
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

        auctionsGroup.MapPost("/{auctionId:Guid}/bids", async (Guid auctionId, CreateBidModel model, AuctionDbContext dbContext, IHubContext<AuctionHub, IAuctionHubClient> hubContext) =>
        {
            var auction = await dbContext.Auctions.Include(x => x.Bids).FirstOrDefaultAsync(x => x.Id == auctionId);
            if (auction is null)
            {
                return Results.NotFound();
            }

            //TODO: remove fake user 
            var user = await dbContext.Users.FirstAsync();
            if (user is null)
            {
                return Results.NotFound();
            }

            auction.AddBid(user, model.Amount);

            await dbContext.SaveChangesAsync();

            await hubContext.Clients.Group(auctionId.ToString()).ReceiveBidUpdate(auctionId, model.Amount);

            //TODO: Return DTO to fix cyclic reference
            return Results.Json(auction, new System.Text.Json.JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            });
        }).WithName("CreateAuctionBid");

    }

}
