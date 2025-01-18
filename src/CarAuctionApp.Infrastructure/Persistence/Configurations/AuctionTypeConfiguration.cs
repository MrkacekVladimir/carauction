using Microsoft.EntityFrameworkCore;
using CarAuctionApp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarAuctionApp.Infrastructure.Persistence.Configurations;

internal sealed class AuctionTypeConfiguration : IEntityTypeConfiguration<Auction>
{
    public void Configure(EntityTypeBuilder<Auction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
          .IsRequired()
          .ValueGeneratedOnAdd();

        builder.HasMany<AuctionBid>(x => x.Bids)
          .WithOne(x => x.Auction)
          .HasForeignKey(x => x.AuctionId)
          .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Bids)
          .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
