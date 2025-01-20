using CarAuctionApp.Domain.Auctions.Entities;

namespace CarAuctionApp.Domain.Auctions.Repositories;

public interface IAuctionRepository
{
    public Task<Auction?> GetById(Guid id);
    public Task AddAsync(Auction auction);
    public Task RemoveAsync(Auction auction);
}
