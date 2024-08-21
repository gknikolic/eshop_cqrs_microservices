namespace BuildingBlocks.Messaging.Events.ProductEvents;
public record ProductUpdatedIntegrationEvent(Guid Id, string Name, decimal Price) : IntegrationEvent;
