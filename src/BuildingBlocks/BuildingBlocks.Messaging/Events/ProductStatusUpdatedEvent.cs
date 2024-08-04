namespace BuildingBlocks.Messaging.Events;
public record ProductStatusUpdatedEvent(Guid Id, bool NewStatus) : IntegrationEvent;

