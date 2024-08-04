namespace BuildingBlocks.Messaging.Events;

public record ProductDeletedEvent(Guid id) : IntegrationEvent;

