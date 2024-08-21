namespace BuildingBlocks.Messaging.Events.ProductEvents;

public record ProductDeletedIntegrationEvent(Guid Id) : IntegrationEvent;

