namespace CarAuctionApp.Persistence.Outbox;

public class OutboxMessage
{
    protected OutboxMessage()
    {
        Type = null!;
        Data = null!;
    }

    public OutboxMessage(string type, string data)
    {
        Id = Guid.NewGuid();
        Type = type;
        Data = data;
        CreatedOn = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Type { get; private set; }
    public string Data { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? ProcessedOn { get; private set; }
}
