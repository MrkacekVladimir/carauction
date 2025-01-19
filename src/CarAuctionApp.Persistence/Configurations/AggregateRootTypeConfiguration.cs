using CarAuctionApp.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarAuctionApp.Persistence.Configurations;

internal abstract class AggregateRootTypeConfiguration<TAggregateRoot> : EntityBaseTypeConfiguration<TAggregateRoot> where TAggregateRoot : AggregateRoot
{
    public override void Configure(EntityTypeBuilder<TAggregateRoot> builder)
    {
        base.Configure(builder);

        builder.Ignore(e => e.DomainEvents);
    }
}
