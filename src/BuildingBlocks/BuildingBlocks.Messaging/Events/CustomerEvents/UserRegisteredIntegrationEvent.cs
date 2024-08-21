namespace BuildingBlocks.Messaging.Events.CustomerEvents;
public record UserRegisteredIntegrationEvent(Guid Id, string Name, string Email, string CreatedBy) : IntegrationEvent;
