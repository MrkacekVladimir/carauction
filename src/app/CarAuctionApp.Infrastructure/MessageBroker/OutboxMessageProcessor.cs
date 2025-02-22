using CarAuctionApp.Contracts;
using CarAuctionApp.Persistence;
using CarAuctionApp.Persistence.Outbox;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CarAuctionApp.Infrastructure.MessageBroker;

internal record OutboxMessageUpdate(Guid Id, string? Error);

public sealed class OutboxMessageProcessor
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly AuctionDbContext _dbContext;

    private const int BatchSize = 100;

    private static Dictionary<string, Type> CachedResolvedTypes = new Dictionary<string, Type>();

    public OutboxMessageProcessor(IPublishEndpoint publishEndpoint, AuctionDbContext dbContext)
    {
        this._publishEndpoint = publishEndpoint;
        this._dbContext = dbContext;
    }

    public async Task Process(CancellationToken cancellationToken)
    {
        var outboxMessages = await _dbContext.OutboxMessages
            .Where(m => m.ProcessedOn == null)
            .OrderBy(m => m.CreatedOn)
            .Take(BatchSize)
            .ToListAsync(cancellationToken);
        if(outboxMessages.Count == 0)
        {
            return;
        }

        var tasks = outboxMessages
            .Select(outboxMessages => PublishMessage(outboxMessages, cancellationToken))
            .ToList();

        await Task.WhenAll(tasks);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task PublishMessage(OutboxMessage outboxMessage, CancellationToken cancellationToken)
    {
        string? error = null;
        try
        {
            var type = GetOrAddType(outboxMessage.Type);
            var @event = JsonSerializer.Deserialize(outboxMessage.Payload, type)!;
            await _publishEndpoint.Publish(@event, cancellationToken);
        }
        catch (Exception ex)
        {
            error = ex.Message;
        }

        outboxMessage.MarkAsProcessed(error);
    }

    private Type GetOrAddType(string messageType)
    {
        if(CachedResolvedTypes.TryGetValue(messageType, out var resolvedType))
        {
            return resolvedType;
        }

        Type type = ContractsAssemblyReference.Assembly.GetType(messageType)!;
        CachedResolvedTypes.Add(messageType, type);
        return type;
    }
}
