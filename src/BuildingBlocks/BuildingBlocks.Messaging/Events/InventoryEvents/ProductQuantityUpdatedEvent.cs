namespace BuildingBlocks.Messaging.Events.InventoryEvents;
public record ProductQuantityUpdatedEvent(Guid ProductId, int Quantity) : IntegrationEvent;
