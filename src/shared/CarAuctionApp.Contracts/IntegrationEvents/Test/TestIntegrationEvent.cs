namespace CarAuctionApp.Contracts.IntegrationEvents.Test;

public record TestIntegrationEvent(Guid Id, string Username): IIntegrationEvent;
