namespace BuildingBlocks.Messaging.Events;

public record ProductCreatedEvent : IntegrationEvent
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public decimal Price { get; set; } = default!;
}
