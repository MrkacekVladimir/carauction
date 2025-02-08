using CarAuctionApp.Persistence.Outbox;
using CarAuctionApp.SharedKernel;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

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
                .Select(OutboxMessage.MapToOutboxMessage)
                .ToList();

            await dbContext.Set<OutboxMessage>().AddRangeAsync(outboxMessages);

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

    }
}
