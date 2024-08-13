namespace BuildingBlocks.Messaging.Events.InventoryEvents;

public record ProductDeletedEvent(Guid id) : IntegrationEvent;

