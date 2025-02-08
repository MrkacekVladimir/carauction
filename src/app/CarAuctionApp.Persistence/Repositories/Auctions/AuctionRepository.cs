using CarAuctionApp.Domain.Auctions.Entities;
using CarAuctionApp.Domain.Auctions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApp.Persistence.Repositories.Auctions;

public class AuctionRepository : IAuctionRepository
{
    private readonly AuctionDbContext _dbContext;

    public AuctionRepository(AuctionDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public Task AddAsync(Auction auction)
    {
        _dbContext.Auctions.Add(auction);
        return Task.CompletedTask;
    }

    public async Task<Auction?> GetById(Guid id)
    {
        return await _dbContext.Auctions
            .Include(x => x.Bids)
                .ThenInclude(y => y.User)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task RemoveAsync(Auction auction)
    {
        _dbContext.Auctions.Remove(auction);
        return Task.CompletedTask;
    }
}
