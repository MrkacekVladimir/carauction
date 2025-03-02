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
using CarAuctionApp.WebApi.Models.Auctions;
using CarAuctionApp.Application.Authentication;
using CarAuctionApp.Application.Features.Auctions.Dtos;

namespace CarAuctionApp.WebApi.Endpoints;

internal static class AuctionsEndpoints
{
    public static void MapAuctionsEndpoints(this WebApplication app)
    {
        var auctionsGroup = app.MapGroup("/auctions").WithTags("Auction");

        auctionsGroup.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            GetAuctionsListResponse result = await mediator.Send(new GetAuctionsListQuery(), cancellationToken);
            return Results.Json(result);
        })
            .WithName("GetAuctions")
            .WithSummary("Gets all of the auctions")
            .WithDescription("Retrieves a collection of auctions from the system.")
            .Produces<GetAuctionsListResponse>();

        auctionsGroup.MapGet("/full-text", async (string searchTerm, IMediator mediator, CancellationToken cancellationToken) =>
        {
            SearchListByFullTextResponse result = await mediator.Send(new SearchListByFullTextQuery(searchTerm), cancellationToken);
            return Results.Json(result);
        })
            .WithName("GetAuctionsByFullText")
            .WithSummary("Gets all of the auctions based on the full text search")
            .WithDescription("Retrieves a collection of auctions from the system based on full text filtering.")
            .Produces<SearchListByFullTextResponse>();


        auctionsGroup.MapPost("/", async (
            CreateAuctionRequest request,
            ICurrentUserProvider currentUserProvider,
            IMediator mediator, CancellationToken cancellationToken) =>
        {
            var user = await currentUserProvider.GetCurrentUserAsync();
            if (user is null)
            {
                return Results.Unauthorized();
            }

            CreateAuctionCommand command = new CreateAuctionCommand(user.Id, request.Title, request.StartsOn, request.EndsOn);
            Result<CreateAuctionResponse> result = await mediator.Send(command, cancellationToken);
            if (!result.IsSuccess)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.CreatedAtRoute("GetAuctionById", new { auctionId = result.Value.Auction.Id }, result.Value);
        })
            .WithName("CreateAuction")
            .WithSummary("Creates an auction")
            .WithDescription("Based on the provided data creates a new auction to the system.")
            .Produces<CreateAuctionResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        auctionsGroup.MapGet("/{auctionId:guid}", async (Guid auctionId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            GetAuctionByIdResponse response = await mediator.Send(new GetAuctionByIdQuery(auctionId), cancellationToken);
            if (response.Auction is null)
            {
                return Results.NotFound();
            }

            return Results.Json(response.Auction);
        })
            .WithName("GetAuctionById")
            .WithSummary("Gets an auction by UUID")
            .WithDescription("Retrieves an auction from the system based on UUID")
            .Produces<AuctionDto>()
            .ProducesProblem(StatusCodes.Status404NotFound);


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

        auctionsGroup.MapDelete("/{auctionId:guid}", async (Guid auctionId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            Result result = await mediator.Send(new DeleteAuctionCommand(auctionId), cancellationToken);
            if (!result.IsSuccess)
            {
                //TODO: fix error codes, shouldnt be magic strings
                return result.Error.Code switch
                {
                    "NotFound" => Results.NotFound(result.Error),
                    _ => Results.BadRequest(result.Error)
                };
            }

            return Results.Ok();
        })
            .WithName("DeleteAuction")
            .WithSummary("Deletes an auction by UUID")
            .WithDescription("Deletes an auction from the system based on UUID")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        auctionsGroup.MapGet("/{auctionId:guid}/bids", async (Guid auctionId, AuctionDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var bids = await dbContext.AuctionBids.Where(b => b.AuctionId == auctionId).ToListAsync(cancellationToken);
            return Results.Json(bids);
        }).WithName("GetAuctionBids");//TODO: OpenAPI metadata

        auctionsGroup.MapPost("/{auctionId:guid}/bids", async (
            Guid auctionId,
            CreateAuctionBidRequest model,
            ICurrentUserProvider currentUserProvider,
            IMediator mediator,
            IHubContext<AuctionHub, IAuctionHubClient> hubContext,
            CancellationToken cancellationToken
            ) =>
        {
            var user = await currentUserProvider.GetCurrentUserAsync();
            if (user is null)
            {
                return Results.Unauthorized();
            }

            CreateAuctionBidCommand command = new CreateAuctionBidCommand(auctionId, user.Id, model.Amount);

            Result<CreateAuctionBidResponse> result = await mediator.Send(command, cancellationToken);
            if (!result.IsSuccess)
            {
                //TODO: fix error codes, shouldnt be magic strings
                return result.Error.Code switch
                {
                    "Unauthorized" => Results.Unauthorized(),
                    "NotFound" => Results.NotFound(result.Error),
                    _ => Results.BadRequest(result.Error)
                };
            }

            AuctionBid bid = result.Value.AuctionBid;

            //Notify all SignalR clients about the new bid
            await hubContext.Clients.Group(auctionId.ToString()).ReceiveBidUpdate(auctionId, bid.Id, bid.Amount.Value, bid.CreatedOn);

            //TODO: Meaningful response, maybe CreatedAt route and also return DTO 
            return Results.Ok();
        })
            .WithName("CreateAuctionBid")
            .WithSummary("Creates a new auction bid")
            .WithDescription("Creates a new bid for auction with passed GUID")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);

    }

}
