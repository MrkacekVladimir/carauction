using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarAuctionApp.Domain.Auctions.Entities;

namespace CarAuctionApp.Persistence.Configurations;

internal sealed class AuctionBidTypeConfiguration : EntityBaseTypeConfiguration<AuctionBid>
{
    public override void Configure(EntityTypeBuilder<AuctionBid> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
          .IsRequired()
          .ValueGeneratedOnAdd();

        builder.OwnsOne(x => x.Amount, y =>
        {
            y.Property(x => x.Value)
                .HasColumnName("Amount")
                .IsRequired();
        });

        builder.HasOne(x => x.User)
        .WithMany()
        .HasForeignKey(x => x.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
