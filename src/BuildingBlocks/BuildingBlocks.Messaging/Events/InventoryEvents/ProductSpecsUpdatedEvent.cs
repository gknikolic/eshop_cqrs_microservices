namespace BuildingBlocks.Messaging.Events.InventoryEvents;
public record ProductSpecsUpdatedEvent(Guid Id, string Name, decimal Price) : IntegrationEvent;
