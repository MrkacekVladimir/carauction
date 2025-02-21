using CarAuctionApp.Domain.Auctions.Entities;

namespace CarAuctionApp.Domain.Auctions.Repositories;

public interface IAuctionRepository
{
    public Task<Auction?> GetById(Guid id, CancellationToken cancellationToken = default);
    public Task AddAsync(Auction auction, CancellationToken cancellationToken = default);
    public Task RemoveAsync(Auction auction, CancellationToken cancellationToken = default);
}
