namespace BuildingBlocks.Messaging.Events.InventoryEvents;
public record ProductStatusUpdatedIntegrationEvent(Guid ProductId, bool IsActive) : IntegrationEvent;

