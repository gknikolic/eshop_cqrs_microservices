namespace BuildingBlocks.Messaging.Events;
public record ProductQuantityUpdatedEvent(Guid Id, int QuantityChangedBy) : IntegrationEvent;
