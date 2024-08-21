namespace BuildingBlocks.Messaging.Events.InventoryEvents;
public record ProductQuantityUpdatedIntegrationEvent(Guid ProductId, int Quantity) : IntegrationEvent;
