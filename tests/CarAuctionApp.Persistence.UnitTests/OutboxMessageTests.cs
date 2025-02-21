using CarAuctionApp.Contracts.IntegrationEvents;
using CarAuctionApp.Persistence.Outbox;

namespace CarAuctionApp.Persistence.UnitTests
{
    public record CustomTestIntegrationEvent(Guid Id, string Username): IIntegrationEvent;

    public class OutboxMessageTests
    {
        [Fact]
        public void MapToOutboxMessage_ShouldPass()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            string username = "TestUsername";
            CustomTestIntegrationEvent integrationEvent = new CustomTestIntegrationEvent(id, username);

            //Act
            OutboxMessage message = OutboxMessage.MapToOutboxMessage(integrationEvent);

            //Assert
            Assert.Equal(integrationEvent.GetType().FullName, message.Type);
            Assert.Equal(string.Format("{{\"Id\":\"{0}\",\"Username\":\"{1}\"}}", id.ToString(), username), message.Payload);
        }
    }
}
