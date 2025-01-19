using CarAuctionApp.Domain.Auctions.Entities;

namespace CarAuctionApp.Domain.Auctions.Repositories;

public interface IAuctionRepository
{
    public Task<Auction?> GetById(Guid id);
    public void Add(Auction auction);
    public void Remove(Auction auction);
}
