using CarAuctionApp.SharedKernel.Domain;

namespace CarAuctionApp.Persistence;

internal class UnitOfWork(AuctionDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync();
    }
}
