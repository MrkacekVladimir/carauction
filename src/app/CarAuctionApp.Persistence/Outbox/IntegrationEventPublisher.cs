using CarAuctionApp.Contracts.IntegrationEvents;
using CarAuctionApp.SharedKernel;

namespace CarAuctionApp.Persistence.Outbox;

internal class IntegrationEventPublisher(AuctionDbContext dbContext) : IIntegrationEventPublisher
{
    public async Task PublishAsync<TIntegrationEvent>(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default) where TIntegrationEvent : IIntegrationEvent
    {
        OutboxMessage message = OutboxMessage.MapToOutboxMessage(integrationEvent);
        await dbContext.OutboxMessages.AddAsync(message, cancellationToken);
    }
}
