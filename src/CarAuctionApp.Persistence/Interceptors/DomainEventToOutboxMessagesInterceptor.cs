using CarAuctionApp.Domain;
using CarAuctionApp.Persistence.Outbox;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace CarAuctionApp.Persistence.Interceptors
{
    public class DomainEventToOutboxMessagesInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            AuctionDbContext? dbContext = (AuctionDbContext?)eventData.Context;
            if(dbContext is null)
            {
                return base.SavingChanges(eventData, result);
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
                .Select(domainEvent => new OutboxMessage(
                    domainEvent.GetType().Name,
                    JsonSerializer.Serialize(domainEvent)
                    )
                )
                .ToList();

            dbContext.OutboxMessages.AddRange(outboxMessages);

            return base.SavingChanges(eventData, result);
        }
    }
}
