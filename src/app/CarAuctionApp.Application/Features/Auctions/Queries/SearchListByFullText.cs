using CarAuctionApp.Application.Features.Auctions.Dtos;
using CarAuctionApp.Application.Features.Auctions.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CarAuctionApp.Application.Features.Auctions.Queries;


public record SearchListByFullTextQuery(string SearchTerm): IRequest<SearchListByFullTextResponse>;

public record SearchListByFullTextResponse(IEnumerable<AuctionListItemDto> Auctions);

public class SearchListByFullTextHandler(IAuctionReadRepository repository): IRequestHandler<SearchListByFullTextQuery, SearchListByFullTextResponse>
{
    public async Task<SearchListByFullTextResponse> Handle(SearchListByFullTextQuery request, CancellationToken cancellationToken)
    {
        var auctions = await repository.SearchListByFullTextAsync(request.SearchTerm, cancellationToken);

        return new SearchListByFullTextResponse(auctions);
    }
}
