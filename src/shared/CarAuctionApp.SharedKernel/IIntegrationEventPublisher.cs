using CarAuctionApp.Contracts.IntegrationEvents;

namespace CarAuctionApp.SharedKernel;

public interface IIntegrationEventPublisher
{
    public Task PublishAsync<TIntegrationEvent>(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        where TIntegrationEvent : IIntegrationEvent;
}
