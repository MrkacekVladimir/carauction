using CarAuctionApp.Domain;
using CarAuctionApp.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Text.Json;

namespace CarAuctionApp.Infrastructure.MessageBroker;

internal record OutboxMessageUpdate(Guid Id, string? Error);
internal record OutboxMessageMeta(Guid Id, string Type, string Payload);

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
        using var transaction = _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var outboxMessages = await _dbContext.OutboxMessages
            .AsNoTracking()
            .Where(m => m.ProcessedOn == null)
            .OrderBy(m => m.CreatedOn)
            .Take(BatchSize)
            .Select(m => new OutboxMessageMeta(m.Id, m.Type, m.Payload))
            .ToListAsync(cancellationToken);
        if(outboxMessages.Count == 0)
        {
            return;
        }

        ConcurrentQueue<OutboxMessageUpdate> updates = new ConcurrentQueue<OutboxMessageUpdate>();

        var tasks = outboxMessages
            .Select(outboxMessages => PublishMessage(updates, outboxMessages, cancellationToken))
            .ToList();

        await Task.WhenAll(tasks);

        var ids = updates.Select(m => m.Id).ToList();
        await _dbContext.OutboxMessages
            .Where(m => ids.Contains(m.Id))
            .ExecuteUpdateAsync(updates => 
                updates.SetProperty(m => m.ProcessedOn, _ => DateTime.UtcNow)
            , cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task PublishMessage(ConcurrentQueue<OutboxMessageUpdate> updates, OutboxMessageMeta outboxMessage, CancellationToken cancellationToken)
    {
        try
        {
            var type = GetOrAddType(outboxMessage.Type);
            var message = JsonSerializer.Deserialize(outboxMessage.Payload, type);
            await _publishEndpoint.Publish(outboxMessage.Payload, cancellationToken);
            updates.Enqueue(new OutboxMessageUpdate(outboxMessage.Id, null));
        }
        catch (Exception ex)
        {
            updates.Enqueue(new OutboxMessageUpdate(outboxMessage.Id, ex.Message));
        }
    }

    private Type GetOrAddType(string messageType)
    {
        if(CachedResolvedTypes.TryGetValue(messageType, out var resolvedType))
        {
            return resolvedType;
        }

        var type = DomainAssemblyReference.Assembly.GetType(messageType)!;
        CachedResolvedTypes.Add(messageType, type);
        return type;
    }
}
