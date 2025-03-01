using CarAuctionApp.Application.Features.Auctions.Dtos;
using CarAuctionApp.Application.Features.Auctions.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarAuctionApp.Application.Features.Auctions.Queries;

public record GetAuctionByIdQuery(Guid Id): IRequest<GetAuctionByIdResponse>;

public record GetAuctionByIdResponse(AuctionDto? Auction);

public class GetAuctionByIdHandler(IAuctionReadRepository repository): IRequestHandler<GetAuctionByIdQuery, GetAuctionByIdResponse>
{
    public async Task<GetAuctionByIdResponse> Handle(GetAuctionByIdQuery request, CancellationToken cancellationToken)
    {
        var auction = await repository.GetAuctionByIdAsync(request.Id, cancellationToken);
        return new GetAuctionByIdResponse(auction);
    }
}
