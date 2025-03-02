using CarAuctionApp.Domain.Auctions.Repositories;
using CarAuctionApp.SharedKernel;
using CarAuctionApp.SharedKernel.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarAuctionApp.Application.Features.Auctions.Commands;

public record DeleteAuctionCommand(Guid AuctionId) : IRequest<Result>;

public class DeleteAuctionHandler(
    IAuctionRepository auctionRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteAuctionCommand, Result>
{

    public async Task<Result> Handle(DeleteAuctionCommand command, CancellationToken cancellationToken)
    {
        var auction = await auctionRepository.GetById(command.AuctionId);
        if (auction is null)
        {
            return Result.Failure(new Error("NotFound", "Account not found."));
        }

        await auctionRepository.RemoveAsync(auction);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
