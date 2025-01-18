using Microsoft.EntityFrameworkCore;
using CarAuctionApp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarAuctionApp.Infrastructure.Persistence.Configurations;

internal sealed class AuctionTypeConfiguration : IEntityTypeConfiguration<Auction>
{
    public void Configure(EntityTypeBuilder<Auction> builder)
    {
    }
}
