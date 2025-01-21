using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using CarAuctionApp.SharedKernel;
using CarAuctionApp.Domain.Auctions.Entities;
using CarAuctionApp.Domain.Auctions.Repositories;
using CarAuctionApp.Domain.Auctions.ValueObjects;
using CarAuctionApp.Persistence;
using CarAuctionApp.WebApi.Hubs;
using CarAuctionApp.Domain.Auctions.Services;
using CarAuctionApp.Application.Authentication;

namespace CarAuctionApp.WebApi.Endpoints;

public record CreateAuctionModel(string Title, DateTime StartsOn, DateTime EndsOn);
public record CreateBidModel(decimal Amount);

public record AuctionBidUserDto(Guid Id, string Username);
public record AuctionBidDto(Guid Id, decimal Amount, AuctionBidUserDto User);
public record AuctionListItemDto(Guid Id, string Title, IEnumerable<AuctionBidDto> bids);

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

        auctionsGroup.MapPost("/", async (CreateAuctionModel model, IUnitOfWork unitOfWork, IAuctionService auctionService, CancellationToken cancellationToken) =>
        {
            var auction = await auctionService.CreateAuctionAsync(model.Title, model.StartsOn, model.EndsOn);
            await unitOfWork.SaveChangesAsync(cancellationToken);
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
            ICurrentUserProvider currentUserProvider,
            IAuctionRepository auctionRepository,
            IUnitOfWork unitOfWork,
            IHubContext<AuctionHub, IAuctionHubClient> hubContext,
            CancellationToken cancellationToken
            ) =>
        {
            var user = await currentUserProvider.GetCurrentUserAsync();
            if (user is null)
            {
                return Results.Unauthorized();
            }

            var auction = await auctionRepository.GetById(auctionId);
            if (auction is null)
            {
                return Results.NotFound();
            }

            BidAmount amount = new(model.Amount);
            auction.AddBid(user, amount);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await hubContext.Clients.Group(auctionId.ToString()).ReceiveBidUpdate(auctionId, model.Amount);

            //TODO: Return DTO to fix cyclic reference
            return Results.Json(auction, new System.Text.Json.JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            });
        }).WithName("CreateAuctionBid");

    }

}
