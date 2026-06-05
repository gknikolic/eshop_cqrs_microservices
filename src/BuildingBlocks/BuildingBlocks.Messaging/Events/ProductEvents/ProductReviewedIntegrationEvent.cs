namespace BuildingBlocks.Messaging.Events.ProductEvents;
public record ProductReviewedIntegrationEvent : IntegrationEvent
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid CustomerId { get; set; } = default!;
    public string CustomerName { get; set; }
    public string Comment { get; set; } = default!;
    public int Rating { get; set; }
    public DateTime CreatedDate { get; set; }
}
