using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarAuctionApp.Domain.Auctions.Entities;

namespace CarAuctionApp.Persistence.Configurations;

internal sealed class AuctionTypeConfiguration : AggregateRootTypeConfiguration<Auction>
{
    public override void Configure(EntityTypeBuilder<Auction> builder)
    {
        builder.ToTable("Auctions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
          .ValueGeneratedNever()
          .IsRequired();

        builder.OwnsOne(x => x.Date, y =>
        {
            y.Property(x => x.StartsOn)
                .HasColumnName("StartsOn")
                .IsRequired();

            y.Property(x => x.EndsOn)
                .HasColumnName("EndsOn")
                .IsRequired();

            y.Property(x => x.LastBidOn)
                .HasColumnName("LastBidOn")
                .IsRequired(false);
        });

        builder.HasMany(x => x.Bids)
          .WithOne(x => x.Auction)
          .HasForeignKey(x => x.AuctionId)
          .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Bids).Metadata.SetField("_bids");
        builder.Navigation(x => x.Bids)
          .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
