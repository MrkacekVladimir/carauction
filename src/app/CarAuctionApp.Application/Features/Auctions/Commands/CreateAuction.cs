using CarAuctionApp.Application.Features.Auctions.Dtos;
using CarAuctionApp.Domain.Auctions.Entities;
using CarAuctionApp.Domain.Auctions.Repositories;
using CarAuctionApp.Domain.Auctions.ValueObjects;
using CarAuctionApp.Domain.Users.Repositories;
using CarAuctionApp.SharedKernel;
using CarAuctionApp.SharedKernel.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarAuctionApp.Application.Features.Auctions.Commands;

public record CreateAuctionCommand(Guid UserId, string Title, DateTime StartsOn, DateTime EndsOn) : IRequest<Result<CreateAuctionResponse>>;

public record CreateAuctionResponse(AuctionDto Auction);

public class CreateAuctionHandler(
    IUserRepository userRepository,
    IAuctionRepository auctionRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateAuctionCommand, Result<CreateAuctionResponse>>
{
    public async Task<Result<CreateAuctionResponse>> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(request.UserId);
        if(user is null)
        {
            //TODO: Have constants based on the error codes
            return Result<CreateAuctionResponse>.Failure(new Error("NotFound", "Bidder not found"));
        }

        var auctionDateResult = AuctionDate.Create(request.StartsOn, request.EndsOn);
        if (!auctionDateResult.IsSuccess || auctionDateResult.Value is null)
        {
            return Result<CreateAuctionResponse>.Failure(auctionDateResult.Error);
        }

        var result = Auction.Create(user, request.Title, auctionDateResult.Value);
        if (!result.IsSuccess)
        {
            return Result<CreateAuctionResponse>.Failure(auctionDateResult.Error);
        }

        var auction = result.Value!;

        await auctionRepository.AddAsync(auction, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        //TODO: Remove null hack, map to DTO
        return Result<CreateAuctionResponse>.Success(new CreateAuctionResponse(null!));

    }
}
