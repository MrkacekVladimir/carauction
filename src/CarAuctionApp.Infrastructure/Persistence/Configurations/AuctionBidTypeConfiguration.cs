using Microsoft.EntityFrameworkCore;
using CarAuctionApp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarAuctionApp.Infrastructure.Persistence.Configurations;

internal sealed class AuctionBidTypeConfiguration : IEntityTypeConfiguration<AuctionBid>
{
    public void Configure(EntityTypeBuilder<AuctionBid> builder)
    {
    }
}
