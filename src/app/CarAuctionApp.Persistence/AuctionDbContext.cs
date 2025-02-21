using CarAuctionApp.Domain.Auctions.Entities;
using CarAuctionApp.Domain.Users.Entities;
using CarAuctionApp.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApp.Persistence;

public class AuctionDbContext : DbContext
{

    public AuctionDbContext(DbContextOptions options) : base(options)
    {
    }

    protected AuctionDbContext()
    {
    }

    public virtual DbSet<Auction> Auctions { get; private set; } = null!;
    public virtual DbSet<AuctionBid> AuctionBids { get; private set; } = null!;
    public virtual DbSet<User> Users { get; private set; } = null!;
    public virtual DbSet<OutboxMessage> OutboxMessages { get; private set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuctionDbContext).Assembly);
    }
}
