using CarAuctionApp.Application.Features.Auctions.Dtos;
using CarAuctionApp.Application.Features.Auctions.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CarAuctionApp.Application.Features.Auctions.Queries;

public record GetAuctionsListQuery: IRequest<GetAuctionsListResponse>;

public record GetAuctionsListResponse(IEnumerable<AuctionListItemDto> Auctions);

public class GetAuctionsListHandler(IAuctionReadRepository repository): IRequestHandler<GetAuctionsListQuery, GetAuctionsListResponse>
{
    public async Task<GetAuctionsListResponse> Handle(GetAuctionsListQuery request, CancellationToken cancellationToken)
    {
        var auctions = await repository.GetAuctionsListAsync(cancellationToken);

        return new GetAuctionsListResponse(auctions);
    }
}
