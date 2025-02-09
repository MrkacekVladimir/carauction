using CarAuctionApp.Domain.Auctions.DomainEvents;
using MassTransit;

namespace CarAuctionApp.Reporting.WebApi.Consumers;

public class AuctionBidCreatedEventConsumer : IConsumer<AuctionBidCreatedEvent>
{
    public Task Consume(ConsumeContext<AuctionBidCreatedEvent> context)
    {
        return Task.CompletedTask;
    }
}
