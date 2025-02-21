using CarAuctionApp.SharedKernel.Domain;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CarAuctionApp.Persistence.Interceptors;

internal class DomainEventInterceptor(IDomainEventDispatcher domainEventDispatcher) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        AuctionDbContext? dbContext = (AuctionDbContext?)eventData.Context;
        if(dbContext is null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        List<IDomainEvent> domainEvents = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.DomainEvents.ToList();

                aggregateRoot.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        foreach(IDomainEvent domainEvent in domainEvents)
        {
            await domainEventDispatcher.DispatchAsync(domainEvent);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

}
