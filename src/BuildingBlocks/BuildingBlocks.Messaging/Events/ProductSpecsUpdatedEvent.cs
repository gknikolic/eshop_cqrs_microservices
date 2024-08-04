namespace BuildingBlocks.Messaging.Events;
public record ProductSpecsUpdatedEvent(Guid Id, string Name, decimal Price) : IntegrationEvent;
