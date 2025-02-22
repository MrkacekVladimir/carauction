namespace CarAuctionApp.SharedKernel.Domain;
public abstract class AggregateRoot: EntityBase
{
    protected List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();
}
