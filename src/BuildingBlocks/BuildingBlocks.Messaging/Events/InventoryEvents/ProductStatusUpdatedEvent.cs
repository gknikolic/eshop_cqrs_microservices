namespace BuildingBlocks.Messaging.Events.InventoryEvents;
public record ProductStatusUpdatedEvent(Guid Id, bool NewStatus) : IntegrationEvent;

