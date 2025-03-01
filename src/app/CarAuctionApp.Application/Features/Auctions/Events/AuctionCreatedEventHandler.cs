using CarAuctionApp.Application.DomainEvents;
using CarAuctionApp.Contracts.IntegrationEvents.Auctions;
using CarAuctionApp.Domain.Auctions.DomainEvents;
using CarAuctionApp.SharedKernel;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CarAuctionApp.Application.Features.Auctions.Events;

public class AuctionCreatedEventHandler(IIntegrationEventPublisher publisher) : INotificationHandler<DomainEventNotification<AuctionCreatedDomainEvent>>
{
    public async Task Handle(DomainEventNotification<AuctionCreatedDomainEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var integrationEvent = new AuctionCreatedIntegrationEvent(domainEvent.AuctionId, domainEvent.Title);

        await publisher.PublishAsync(integrationEvent, cancellationToken);
    }
}
