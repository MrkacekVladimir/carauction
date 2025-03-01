using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using CarAuctionApp.Domain.Auctions.Repositories;
using CarAuctionApp.Persistence;
using CarAuctionApp.WebApi.Hubs;
using MediatR;
using CarAuctionApp.Domain.Auctions.Entities;
using CarAuctionApp.SharedKernel.Domain;
using CarAuctionApp.Application.Features.Auctions.Commands;
using CarAuctionApp.Application.Features.Auctions.Queries;
using CarAuctionApp.SharedKernel;

namespace CarAuctionApp.WebApi.Endpoints;

internal static class AuctionEndpoints
{
    public static void MapAuctionEndpoints(this WebApplication app)
    {
        var auctionsGroup = app.MapGroup("/auctions").WithTags("Auction");

        auctionsGroup.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetAuctionsListQuery(), cancellationToken);
            return Results.Json(result);
        })
            .WithName("GetAuctions")
            .WithSummary("Gets all of the auctions")
            .WithDescription("Retrieves a collection of auctions from the system.");

        auctionsGroup.MapGet("/full-text", async (string searchTerm, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new SearchListByFullTextQuery(searchTerm), cancellationToken);
            return Results.Json(result);
        })
            .WithName("GetAuctionsByFullText")
            .WithSummary("Gets all of the auctions based on the full text search")
            .WithDescription("Retrieves a collection of auctions from the system based on full text filtering.");


        auctionsGroup.MapPost("/", async (CreateAuctionCommand command, IMediator mediator, CancellationToken cancellationToken) =>
        {
            Auction auction = await mediator.Send(command, cancellationToken);
            return Results.Json(auction);
        })
            .WithName("CreateAuction")
            .WithSummary("Creates an auction")
            .WithDescription("Based on the provided data creates a new auction to the system.");

        auctionsGroup.MapGet("/{auctionId:guid}", async (Guid auctionId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            GetAuctionByIdResponse response = await mediator.Send(new GetAuctionByIdQuery(auctionId), cancellationToken);
            if(response.Auction is null)
            {
                return Results.NotFound();
            }

            return Results.Json(response.Auction);
        })
            .WithName("GetAuctionById")
            .WithSummary("Gets an auction by UUID")
            .WithDescription("Retrieves an auction from the system based on UUID");
        

        auctionsGroup.MapPut("/{auctionId:guid}", async (Guid auctionId, CreateAuctionCommand model, IUnitOfWork unitOfWork, IAuctionRepository auctionRepository, CancellationToken cancellationToken) =>
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
            CreateAuctionBidCommand model,
            IMediator mediator,
            IHubContext<AuctionHub, IAuctionHubClient> hubContext,
            CancellationToken cancellationToken
            ) =>
        {
            Result<CreateAuctionBidResponse> result = await mediator.Send(model, cancellationToken);
            if(!result.IsSuccess)
            {
                return result.Error.Code switch
                {
                    "Unauthorized" => Results.Unauthorized(),
                    "NotFound" => Results.NotFound(result.Error),
                    _ => Results.BadRequest(result.Error)
                };
            }

            //TODO: Move sending price update to seperate service, because now we only send update to connected users to this application
            //This will cause issues when we have multiple instances of this application
            AuctionBid bid = result.Value.AuctionBid;
            await hubContext.Clients.Group(auctionId.ToString()).ReceiveBidUpdate(auctionId, bid.Id, bid.Amount.Value, bid.CreatedOn);

            //TODO: Meaningful response, maybe CreatedAt route and also return DTO 
            return Results.Ok();
        }).WithName("CreateAuctionBid")
        .Produces(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound);

    }

}
