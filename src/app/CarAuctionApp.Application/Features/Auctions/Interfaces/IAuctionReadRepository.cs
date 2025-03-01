using CarAuctionApp.Application.Features.Auctions.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CarAuctionApp.Application.Features.Auctions.Interfaces;

public interface IAuctionReadRepository
{
    Task<AuctionDto?> GetAuctionByIdAsync(Guid auctionId, CancellationToken cancellationToken);
    Task<IEnumerable<AuctionListItemDto>> GetAuctionsListAsync(CancellationToken cancellationToken);
    Task<IEnumerable<AuctionListItemDto>> SearchListByFullTextAsync(string search, CancellationToken cancellationToken);

}
