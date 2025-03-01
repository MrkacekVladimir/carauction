using CarAuctionApp.Contracts.IntegrationEvents.Auctions;
using MassTransit;

namespace CarAuctionApp.Reporting.WebApi.Consumers;

public class AuctionBidCreatedEventConsumer : IConsumer<AuctionBidCreatedIntegrationEvent>
{
    public Task Consume(ConsumeContext<AuctionBidCreatedIntegrationEvent> context)
    {
        return Task.CompletedTask;
    }
}
