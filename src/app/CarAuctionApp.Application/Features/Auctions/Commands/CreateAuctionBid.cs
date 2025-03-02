using CarAuctionApp.Domain.Auctions.Entities;
using CarAuctionApp.Domain.Auctions.Repositories;
using CarAuctionApp.Domain.Auctions.ValueObjects;
using CarAuctionApp.Domain.Users.Entities;
using CarAuctionApp.Domain.Users.Repositories;
using CarAuctionApp.SharedKernel;
using CarAuctionApp.SharedKernel.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarAuctionApp.Application.Features.Auctions.Commands;

public record CreateAuctionBidCommand(Guid AuctionId, Guid UserId, decimal Amount) : IRequest<Result<CreateAuctionBidResponse>>;

public record CreateAuctionBidResponse(AuctionBid AuctionBid);


public class CreateAuctionBidCommandHandler(
            IAuctionRepository auctionRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork
            ) : IRequestHandler<CreateAuctionBidCommand, Result<CreateAuctionBidResponse>>
{
    public async Task<Result<CreateAuctionBidResponse>> Handle(CreateAuctionBidCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetById(command.UserId);
        if (user is null)
        {
            //TODO: Have constants based on the error codes
            return Result<CreateAuctionBidResponse>.Failure(new Error("NotFound", "User not found"));
        }

        Auction? auction = await auctionRepository.GetById(command.AuctionId);
        if (auction is null)
        {
            //TODO: Have constants based on the error codes
            return Result<CreateAuctionBidResponse>.Failure(new Error("NotFound", "Auction not found"));
        }

        BidAmount amount = new(command.Amount);
        Result<AuctionBid?> result = auction.AddBid(user, amount);
        if (!result.IsSuccess)
        {
            return Result<CreateAuctionBidResponse>.Failure(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateAuctionBidResponse>.Success(new CreateAuctionBidResponse(result.Value!));
    }
}
