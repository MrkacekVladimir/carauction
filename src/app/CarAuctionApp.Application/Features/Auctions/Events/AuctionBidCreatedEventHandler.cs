using CarAuctionApp.Application.DomainEvents;
using CarAuctionApp.Contracts.IntegrationEvents.Auctions;
using CarAuctionApp.Domain.Auctions.DomainEvents;
using CarAuctionApp.SharedKernel;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CarAuctionApp.Application.Features.Auctions.Events;

public class AuctionBidCreatedEventHandler(IIntegrationEventPublisher publisher) : INotificationHandler<DomainEventNotification<AuctionBidCreatedDomainEvent>>
{
    public async Task Handle(DomainEventNotification<AuctionBidCreatedDomainEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var integrationEvent = new AuctionBidCreatedIntegrationEvent(domainEvent.BidId, domainEvent.AuctionId, domainEvent.Amount);

        await publisher.PublishAsync(integrationEvent, cancellationToken);
    }
}
