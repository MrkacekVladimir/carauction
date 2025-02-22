using CarAuctionApp.Domain.Auctions.DomainEvents;
using MassTransit;

namespace CarAuctionApp.Reporting.WebApi.Consumers;

public class AuctionBidCreatedEventConsumer : IConsumer<AuctionBidCreatedDomainEvent>
{
    public Task Consume(ConsumeContext<AuctionBidCreatedDomainEvent> context)
    {
        return Task.CompletedTask;
    }
}
