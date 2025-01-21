using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using CarAuctionApp.SharedKernel;
using CarAuctionApp.Domain.Auctions.Repositories;
using CarAuctionApp.Domain.Auctions.ValueObjects;
using CarAuctionApp.Persistence;
using CarAuctionApp.WebApi.Hubs;
using CarAuctionApp.Domain.Auctions.Services;
using CarAuctionApp.Application.Authentication;

namespace CarAuctionApp.WebApi.Endpoints;

//TODO: Move records to separate files
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

        auctionsGroup.MapGet("/", async (AuctionDbContext dbContext, CancellationToken cancellationToken) =>
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
            )).ToListAsync(cancellationToken);

            return Results.Json(auctions);

        }).WithName("GetAuctions");
        auctionsGroup.MapPost("/", async (CreateAuctionModel model, IUnitOfWork unitOfWork, IAuctionService auctionService, CancellationToken cancellationToken) =>
        {
            var auction = await auctionService.CreateAuctionAsync(model.Title, model.StartsOn, model.EndsOn);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Results.Json(auction);
        }).WithName("CreateAuction");
        auctionsGroup.MapGet("/{auctionId:guid}", async (Guid auctionId, AuctionDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var auctionDto = await dbContext.Auctions.AsNoTracking()
                .Select(a => a) //TODO: Create DTO
                .FirstOrDefaultAsync(a => a.Id == auctionId);
            return Results.Json(auctionDto);
        }).WithName("GetAuctionById");
        auctionsGroup.MapPut("/{auctionId:guid}", async (Guid auctionId, CreateAuctionModel model, IUnitOfWork unitOfWork, IAuctionRepository auctionRepository, CancellationToken cancellationToken) =>
        {
            var auction = await auctionRepository.GetById(auctionId);
            if(auction is null)
            {
                return Results.NotFound();
            }

            //TODO: Update auction

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Results.Json(auction);
        }).WithName("UpdateAuctionById");
        auctionsGroup.MapDelete("/{auctionId:guid}", async (Guid auctionId, IUnitOfWork unitOfWork, IAuctionRepository auctionRepository, CancellationToken cancellationToken) =>
        {
            var auction = await auctionRepository.GetById(auctionId);
            if(auction is null)
            {
                return Results.NotFound();
            }

            await auctionRepository.RemoveAsync(auction);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Results.Ok();
        }).WithName("DeleteAuction");
        auctionsGroup.MapGet("/{auctionId:guid}/bids", async (Guid auctionId, AuctionDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var bids = await dbContext.AuctionBids.Where(b => b.AuctionId == auctionId).ToListAsync(cancellationToken);
            return Results.Json(bids);
        }).WithName("GetAuctionBids");
        auctionsGroup.MapPost("/{auctionId:guid}/bids", async (
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
            var result = auction.AddBid(user, amount);
            if(!result.IsSuccess)
            {
                return Results.BadRequest(result.Error);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            //TODO: Move sending price update to seperate service, because now we only send update to connected users to this application
            //This will cause issues when we have multiple instances of this application
            await hubContext.Clients.Group(auctionId.ToString()).ReceiveBidUpdate(auctionId, model.Amount);

            //TODO: Meaningful response, maybe CreatedAt route and also return DTO 
            return Results.Ok();
        }).WithName("CreateAuctionBid");

    }

}
