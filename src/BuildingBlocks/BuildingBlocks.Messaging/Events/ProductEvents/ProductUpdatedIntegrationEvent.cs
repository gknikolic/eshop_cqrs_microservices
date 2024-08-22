namespace BuildingBlocks.Messaging.Events.ProductEvents;
public record ProductUpdatedIntegrationEvent : IntegrationEvent
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public decimal Price { get; set; } = default!;
    public List<string> Categories { get; set; } = new();
    public string Description { get; set; } = default!;
    public List<string> ImagePaths { get; set; } = default!;
    public int PeicesInStock { get; set; }
    public bool IsActive { get; set; }
}
