using CarAuctionApp.Contracts.IntegrationEvents;
using System.Text.Json;

namespace CarAuctionApp.Persistence.Outbox;

public class OutboxMessage
{
#pragma warning disable CS8618
    // EF Core
    private OutboxMessage()
    {
    }
#pragma warning restore CS8618

    public OutboxMessage(string type, string payload)
    {
        Id = Guid.NewGuid();
        Type = type;
        Payload = payload;
        CreatedOn = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Type { get; private set; }
    public string Payload { get; private set; }
    public string? Error { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? ProcessedOn { get; private set; }

    public void MarkAsProcessed(string? error = null)
    {
        Error = error;
        ProcessedOn = DateTime.UtcNow;
    }

    public static OutboxMessage MapToOutboxMessage(IIntegrationEvent integrationEvent)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = false,
        };

        var type = integrationEvent.GetType();
        return new OutboxMessage(
            type.FullName!,
            JsonSerializer.Serialize(integrationEvent, type, options)
        );
    }
}
