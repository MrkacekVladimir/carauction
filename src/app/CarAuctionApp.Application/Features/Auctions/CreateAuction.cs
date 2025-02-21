using CarAuctionApp.Domain.Auctions.Entities;
using CarAuctionApp.Domain.Auctions.Repositories;
using CarAuctionApp.Domain.Auctions.ValueObjects;
using CarAuctionApp.SharedKernel.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarAuctionApp.Application.Features.Auctions;

public record CreateAuctionCommand(string Title, DateTime StartsOn, DateTime EndsOn) : IRequest<Auction>;

public class CreateAuctionHandler(IAuctionRepository auctionRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateAuctionCommand, Auction>
{
    public async Task<Auction> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
    {
        var auctionDateResult = AuctionDate.Create(request.StartsOn, request.EndsOn);
        if (!auctionDateResult.IsSuccess || auctionDateResult.Value == null)
        {
            //TODO: Remove exceptions
            throw new Exception(auctionDateResult.Error.ToString());
        }

        var result = Auction.Create(request.Title, auctionDateResult.Value);
        if (!result.IsSuccess)
        {
            //TODO: Remove exceptions
            throw new Exception(result.Error.ToString());
        }

        var auction = result.Value!;

        await auctionRepository.AddAsync(auction, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return auction;

    }
}
