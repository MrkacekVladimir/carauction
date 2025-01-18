using Microsoft.EntityFrameworkCore;
using CarAuctionApp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarAuctionApp.Infrastructure.Persistence.Configurations;

internal sealed class AuctionBidTypeConfiguration : IEntityTypeConfiguration<AuctionBid>
{
    public void Configure(EntityTypeBuilder<AuctionBid> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
          .IsRequired()
          .ValueGeneratedOnAdd();

        builder.HasOne(x => x.User)
        .WithMany()
        .HasForeignKey(x => x.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
