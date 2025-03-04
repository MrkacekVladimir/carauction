﻿using CarAuctionApp.SharedKernel.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarAuctionApp.Application.DomainEvents;

internal class DomainEventDispatcher(ILogger<DomainEventDispatcher> logger, IMediator mediator) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = Activator.CreateInstance(typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent);
        if (notification == null)
        {
            logger.LogWarning("Failed to create DomainEventNotification for domain event {DomainEvent}", domainEvent);
            return;
        }

        await mediator.Publish(notification, cancellationToken);
    }

    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        List<IDomainEvent> events = domainEvents.ToList();

        foreach (IDomainEvent @event in events)
        {
            await DispatchAsync(@event, cancellationToken);
        }
    }
}
