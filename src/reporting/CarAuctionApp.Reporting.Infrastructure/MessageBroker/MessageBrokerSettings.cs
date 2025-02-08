namespace CarAuctionApp.Reporting.Infrastructure.MessageBroker;

public sealed class MessageBrokerSettings
{
    public string Host { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
