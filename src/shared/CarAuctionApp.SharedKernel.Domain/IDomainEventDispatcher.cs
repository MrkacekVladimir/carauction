namespace CarAuctionApp.SharedKernel.Domain;

public interface IDomainEventDispatcher
{
    public Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
    public Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
