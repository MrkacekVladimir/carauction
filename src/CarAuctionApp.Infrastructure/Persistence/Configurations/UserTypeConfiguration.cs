using Microsoft.EntityFrameworkCore;
using CarAuctionApp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarAuctionApp.Infrastructure.Persistence.Configurations;

internal sealed class UserTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
          .IsRequired()
          .ValueGeneratedOnAdd();
    }
}
