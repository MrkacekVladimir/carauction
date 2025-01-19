using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarAuctionApp.Domain.Users.Entities;

namespace CarAuctionApp.Persistence.Configurations;

internal sealed class UserTypeConfiguration : EntityBaseTypeConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
          .IsRequired()
          .ValueGeneratedOnAdd();
    }
}
