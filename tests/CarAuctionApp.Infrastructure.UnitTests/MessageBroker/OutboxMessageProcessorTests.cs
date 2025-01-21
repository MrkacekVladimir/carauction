using CarAuctionApp.Domain.Users.DomainEvents;
using CarAuctionApp.Infrastructure.MessageBroker;
using CarAuctionApp.Persistence;
using CarAuctionApp.Persistence.Outbox;
using CarAuctionApp.SharedKernel;
using MassTransit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System.Data.Common;

namespace CarAuctionApp.Infrastructure.UnitTests.MessageBroker
{
    public class OutboxMessageProcessorTests
    {
        private readonly AuctionDbContext _dbContext;

        private readonly OutboxMessageProcessor _outboxMessageProcessor;
        private readonly IPublishEndpoint _publishEndpoint;
        public OutboxMessageProcessorTests()
        {
            _publishEndpoint = Substitute.For<IPublishEndpoint>();

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var contextOptions = new DbContextOptionsBuilder<AuctionDbContext>()
                .UseSqlite(connection)
                .Options;

            _dbContext = new AuctionDbContext(contextOptions);
            _dbContext.Database.EnsureCreated();
            _outboxMessageProcessor = new OutboxMessageProcessor(_publishEndpoint, _dbContext);
        }

        [Fact]
        public async Task Process_Should_ReturnImmediatelyWithNoMessages()
        {
            //Arrange/Act
            await _outboxMessageProcessor.Process(CancellationToken.None);

            //Assert
            await _publishEndpoint.DidNotReceive().Publish(Arg.Any<string>(), Arg.Any<CancellationToken>());
            Assert.Empty(_dbContext.OutboxMessages.ToList());
        }

        [Fact]
        public async Task Process_Should_PublishAllWithoutErrors()
        {
            //Arrange
            List<IDomainEvent> events = [
                new UserCreatedEvent(Guid.NewGuid(), "Test1"),
                new UserCreatedEvent(Guid.NewGuid(), "Test2")
                ];
            var outboxMessages = events.Select(OutboxMessage.MapToOutboxMessage);
            _dbContext.OutboxMessages.AddRange(outboxMessages);
            _dbContext.SaveChanges();

            //Act
            await _outboxMessageProcessor.Process(CancellationToken.None);

            //Assert
            await _publishEndpoint.Received(events.Count).Publish(Arg.Any<object>(), Arg.Any<CancellationToken>());
            Assert.True(_dbContext.OutboxMessages.All(m => m.ProcessedOn != null && m.Error == null));
        }
    }
}
