namespace BuildingBlocks.Messaging.Events.CustomerEvents;
public record UserRegisteredEvent(Guid Id, string Name, string Email, string CreatedBy) : IntegrationEvent;
