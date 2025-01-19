using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarAuctionApp.Domain.Auctions.Entities;

namespace CarAuctionApp.Persistence.Configurations;

internal sealed class AuctionTypeConfiguration : AggregateRootTypeConfiguration<Auction>
{
    public override void Configure(EntityTypeBuilder<Auction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
          .IsRequired()
          .ValueGeneratedOnAdd();

        builder.HasMany(x => x.Bids)
          .WithOne(x => x.Auction)
          .HasForeignKey(x => x.AuctionId)
          .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Bids)
          .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
