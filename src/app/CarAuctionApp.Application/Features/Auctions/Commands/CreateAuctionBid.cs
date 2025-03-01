using CarAuctionApp.Application.Authentication;
using CarAuctionApp.Domain.Auctions.Entities;
using CarAuctionApp.Domain.Auctions.Repositories;
using CarAuctionApp.Domain.Auctions.ValueObjects;
using CarAuctionApp.SharedKernel;
using CarAuctionApp.SharedKernel.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarAuctionApp.Application.Features.Auctions.Commands;

public record CreateAuctionBidCommand(Guid AuctionId, decimal Amount) : IRequest<Result<CreateAuctionBidResponse>>;

public record CreateAuctionBidResponse(AuctionBid AuctionBid);


public class CreateAuctionBidCommandHandler(
            ICurrentUserProvider currentUserProvider,
            IAuctionRepository auctionRepository,
            IUnitOfWork unitOfWork) : IRequestHandler<CreateAuctionBidCommand, Result<CreateAuctionBidResponse>>
{
    public async Task<Result<CreateAuctionBidResponse>> Handle(CreateAuctionBidCommand request, CancellationToken cancellationToken)
    {
        var user = await currentUserProvider.GetCurrentUserAsync();
        if (user is null)
        {
            //TODO: Have constants based on the error codes
            return Result<CreateAuctionBidResponse>.Failure(new Error("Unauthorized", "You are not authorized."));
        }

        var auction = await auctionRepository.GetById(request.AuctionId);
        if (auction is null)
        {
            //TODO: Have constants based on the error codes
            return Result<CreateAuctionBidResponse>.Failure(new Error("NotFound", "Auction not found"));
        }

        BidAmount amount = new(request.Amount);
        var result = auction.AddBid(user, amount);
        if (!result.IsSuccess)
        {
            return Result<CreateAuctionBidResponse>.Failure(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateAuctionBidResponse>.Success(new CreateAuctionBidResponse(result.Value!));
    }
}
