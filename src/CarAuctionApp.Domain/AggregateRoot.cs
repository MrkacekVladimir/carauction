namespace CarAuctionApp.Domain;

public interface IDomainEvent;
public class AggregateRoot
{
    protected List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();
}
