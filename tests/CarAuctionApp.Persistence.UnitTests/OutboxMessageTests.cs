using CarAuctionApp.Persistence.Outbox;
using CarAuctionApp.SharedKernel;

namespace CarAuctionApp.Persistence.UnitTests
{
    public record CustomTestEvent(Guid Id, string Username): IDomainEvent;

    public class OutboxMessageTests
    {
        [Fact]
        public void MapToOutboxMessage_ShouldPass()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            string username = "TestUsername";
            IDomainEvent domainEvent = new CustomTestEvent(id, username);

            //Act
            OutboxMessage message = OutboxMessage.MapToOutboxMessage(domainEvent);

            //Assert
            Assert.Equal(nameof(CustomTestEvent), message.Type);
            Assert.Equal(string.Format("{{\"Id\":\"{0}\",\"Username\":\"{1}\"}}", id.ToString(), username), message.Payload);
        }
    }
}
