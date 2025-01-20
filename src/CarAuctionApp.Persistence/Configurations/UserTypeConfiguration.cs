using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarAuctionApp.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApp.Persistence.Configurations;

internal sealed class UserTypeConfiguration : EntityBaseTypeConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
          .ValueGeneratedNever()
          .IsRequired();
    }
}
