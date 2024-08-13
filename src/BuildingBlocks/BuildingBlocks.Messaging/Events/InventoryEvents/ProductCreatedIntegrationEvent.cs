namespace BuildingBlocks.Messaging.Events.InventoryEvents;

public record ProductCreatedIntegrationEvent : IntegrationEvent
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public decimal Price { get; set; } = default!;
}
