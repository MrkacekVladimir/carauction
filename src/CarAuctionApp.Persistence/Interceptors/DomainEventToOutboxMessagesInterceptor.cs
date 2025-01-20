using CarAuctionApp.Domain;
using CarAuctionApp.Persistence.Outbox;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace CarAuctionApp.Persistence.Interceptors
{
    public class DomainEventToOutboxMessagesInterceptor : SaveChangesInterceptor
    {
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            AuctionDbContext? dbContext = (AuctionDbContext?)eventData.Context;
            if(dbContext is null)
            {
                return await base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            List<OutboxMessage> outboxMessages = dbContext.ChangeTracker
                .Entries<AggregateRoot>()
                .Select(x => x.Entity)
                .SelectMany(aggregateRoot =>
                {
                    var domainEvents = aggregateRoot.DomainEvents.ToList();

                    aggregateRoot.ClearDomainEvents();

                    return domainEvents;
                })
                .Select(MapToOutboxMessage)
                .ToList();

            await dbContext.Set<OutboxMessage>().AddRangeAsync(outboxMessages);

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private OutboxMessage MapToOutboxMessage(IDomainEvent domainEvent)
        {
            return new OutboxMessage(
                domainEvent.GetType().Name,
                JsonSerializer.Serialize(domainEvent)
            );
        }
    }
}
