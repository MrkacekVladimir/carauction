namespace CarAuctionApp.Contracts.IntegrationEvents;

public record TestIntegrationEvent(Guid Id, string Username): IIntegrationEvent;
