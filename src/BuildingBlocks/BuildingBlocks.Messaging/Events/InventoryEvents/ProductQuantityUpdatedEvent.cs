namespace BuildingBlocks.Messaging.Events.InventoryEvents;
public record ProductQuantityUpdatedEvent(Guid Id, int Quantity) : IntegrationEvent;
