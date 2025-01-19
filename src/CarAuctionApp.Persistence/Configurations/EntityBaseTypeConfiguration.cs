using CarAuctionApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarAuctionApp.Persistence.Configurations;

internal abstract class EntityBaseTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.CreatedOn)
            .IsRequired();

        builder.Property(e => e.LastUpdatedOn)
            .IsRequired(false);
    }
}
