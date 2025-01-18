using CarAuctionApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApp.Infrastructure.Persistence;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions options) : base(options)
    {
    }

    protected AuctionDbContext()
    {
    }

    public DbSet<Auction> Auctions { get; private set; } = null!;
    public DbSet<AuctionBid> AuctionBids { get; private set; } = null!;
    public DbSet<User> Users { get; private set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuctionDbContext).Assembly);
    }
}
