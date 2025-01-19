using CarAuctionApp.Domain.Auctions.Entities;
using CarAuctionApp.Domain.Auctions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApp.Persistence.Repositories.Auctions;

public class AuctionRepository : IAuctionRepository
{
    private readonly AuctionDbContext dbContext;

    public AuctionRepository(AuctionDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Add(Auction auction)
    {
        dbContext.Auctions.Add(auction);
    }

    public Task<Auction?> GetById(Guid id)
    {
        return dbContext.Auctions
            .Include(x => x.Bids)
                .ThenInclude(y => y.User)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Remove(Auction auction)
    {
        dbContext.Auctions.Remove(auction);
    }
}
