namespace BuildingBlocks.Messaging.Events.InventoryEvents;
public record ProductStatusUpdatedIntegrationEvent(Guid Id, bool NewStatus) : IntegrationEvent;

