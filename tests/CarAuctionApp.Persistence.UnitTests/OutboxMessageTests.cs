using CarAuctionApp.Contracts.IntegrationEvents.Test;
using CarAuctionApp.Persistence.Outbox;

namespace CarAuctionApp.Persistence.UnitTests
{

    public class OutboxMessageTests
    {
        [Fact]
        public void MapToOutboxMessage_ShouldPass()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            string username = "TestUsername";
            TestIntegrationEvent integrationEvent = new TestIntegrationEvent(id, username);

            //Act
            OutboxMessage message = OutboxMessage.MapToOutboxMessage(integrationEvent);

            //Assert
            Assert.Equal(integrationEvent.GetType().FullName, message.Type);
            Assert.Equal(string.Format("{{\"Id\":\"{0}\",\"Username\":\"{1}\"}}", id.ToString(), username), message.Payload);
        }
    }
}
