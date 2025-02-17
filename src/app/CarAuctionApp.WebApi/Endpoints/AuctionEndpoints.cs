using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using CarAuctionApp.SharedKernel;
using CarAuctionApp.Domain.Auctions.Repositories;
using CarAuctionApp.Domain.Auctions.ValueObjects;
using CarAuctionApp.Persistence;
using CarAuctionApp.WebApi.Hubs;
using CarAuctionApp.Domain.Auctions.Services;
using CarAuctionApp.Application.Authentication;
using CarAuctionApp.WebApi.Models.Auction;

namespace CarAuctionApp.WebApi.Endpoints;

internal static class AuctionEndpoints
{
    public static void MapAuctionEndpoints(this WebApplication app)
    {
        var auctionsGroup = app.MapGroup("/auctions").WithTags("Auction");

        auctionsGroup.MapGet("/", async (AuctionDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var auctions = await dbContext.Auctions.AsNoTracking()
            .Select(a =>
            new AuctionListItemDto(
                a.Id,
                a.Title,
                a.Date.StartsOn,
                a.Date.EndsOn,
                a.Bids.Select(b => new AuctionBidDto(
                    b.Id,
                    b.Amount.Value,
                    b.CreatedOn,
                    new AuctionBidUserDto(b.User.Id, b.User.Username))
                )
            )).ToListAsync(cancellationToken);

            var result = new GetAuctionsResponse(auctions);
            return Results.Json(result);

        })
            .WithName("GetAuctions")
            .WithSummary("Gets all of the auctions")
            .WithDescription("Retrieves a collection of auctions from the system.");

        auctionsGroup.MapGet("/full-text", async (string search, AuctionDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var auctions = await dbContext.Auctions.AsNoTracking()
            .Where(a => EF.Functions.ToTsVector("english", a.Title + " " + a.Description) //TODO: Abstract this Postgres specific logic
                .Matches(EF.Functions.PhraseToTsQuery("english", search)))
            .Select(a =>
            new AuctionListItemDto(
                a.Id,
                a.Title,
                a.Date.StartsOn,
                a.Date.EndsOn,
                a.Bids.Select(b => new AuctionBidDto(
                    b.Id,
                    b.Amount.Value,
                    b.CreatedOn,
                    new AuctionBidUserDto(b.User.Id, b.User.Username))
                )
            )).ToListAsync(cancellationToken);

            var result = new GetAuctionsResponse(auctions);
            return Results.Json(result);

        })
            .WithName("GetAuctionsByFullText")
            .WithSummary("Gets all of the auctions based on the full text search")
            .WithDescription("Retrieves a collection of auctions from the system based on full text filtering.");


        auctionsGroup.MapPost("/", async (CreateAuctionRequest model, IUnitOfWork unitOfWork, IAuctionService auctionService, CancellationToken cancellationToken) =>
        {
            var auction = await auctionService.CreateAuctionAsync(model.Title, model.StartsOn, model.EndsOn);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Results.Json(auction);
        })
            .WithName("CreateAuction")
            .WithSummary("Creates an auction")
            .WithDescription("Based on the provided data creates a new auction to the system.");

        auctionsGroup.MapGet("/{auctionId:guid}", async (Guid auctionId, AuctionDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var data = await dbContext.Auctions.AsNoTracking()
                .Where(a => a.Id == auctionId)
                .Select(a => new
                {
                    a.Id,
                    a.Title,
                    StartsOn = a.Date.StartsOn,
                    EndsOn = a.Date.EndsOn,
                    Bids = a.Bids.OrderByDescending(b => b.Amount.Value)
                    .Select(b => new
                    {
                        b.Id,
                        Amount = b.Amount.Value,
                        b.CreatedOn,
                        User = new { b.User.Id, b.User.Username }
                    })
                })
                .FirstOrDefaultAsync();

            if (data is null)
            {
                return Results.NotFound();
            }

            var auctionDto = new AuctionDto(
                data.Id,
                data.Title,
                data.StartsOn,
                data.EndsOn,
                data.Bids.Select(b => new AuctionBidDto(
                    b.Id,
                    b.Amount,
                    b.CreatedOn,
                    new AuctionBidUserDto(b.User.Id, b.User.Username)
                ))
            );

            var result = new GetAuctionResponse(auctionDto);
            return Results.Json(result);
        })
            .WithName("GetAuctionById")
            .WithSummary("Gets an auction by UUID")
            .WithDescription("Retrieves an auction from the system based on UUID");

        auctionsGroup.MapPut("/{auctionId:guid}", async (Guid auctionId, CreateAuctionRequest model, IUnitOfWork unitOfWork, IAuctionRepository auctionRepository, CancellationToken cancellationToken) =>
        {
            var auction = await auctionRepository.GetById(auctionId);
            if (auction is null)
            {
                return Results.NotFound();
            }

            //TODO: Update auction

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Results.Json(auction);
        }).WithName("UpdateAuctionById"); //TODO: OpenAPI metadata

        auctionsGroup.MapDelete("/{auctionId:guid}", async (Guid auctionId, IUnitOfWork unitOfWork, IAuctionRepository auctionRepository, CancellationToken cancellationToken) =>
        {
            var auction = await auctionRepository.GetById(auctionId);
            if (auction is null)
            {
                return Results.NotFound();
            }

            await auctionRepository.RemoveAsync(auction);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Results.Ok();
        }).WithName("DeleteAuction"); //TODO: OpenAPI metadata

        auctionsGroup.MapGet("/{auctionId:guid}/bids", async (Guid auctionId, AuctionDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var bids = await dbContext.AuctionBids.Where(b => b.AuctionId == auctionId).ToListAsync(cancellationToken);
            return Results.Json(bids);
        }).WithName("GetAuctionBids");//TODO: OpenAPI metadata

        auctionsGroup.MapPost("/{auctionId:guid}/bids", async (
            Guid auctionId,
            CreateBidRequest model,
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
            if (!result.IsSuccess)
            {
                return Results.BadRequest(result.Error);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var bid = result.Value!;

            //TODO: Move sending price update to seperate service, because now we only send update to connected users to this application
            //This will cause issues when we have multiple instances of this application
            await hubContext.Clients.Group(auctionId.ToString()).ReceiveBidUpdate(auctionId, bid.Id, bid.Amount.Value, bid.CreatedOn);

            //TODO: Meaningful response, maybe CreatedAt route and also return DTO 
            return Results.Ok();
        }).WithName("CreateAuctionBid");//TODO: OpenAPI metadata

    }

}
