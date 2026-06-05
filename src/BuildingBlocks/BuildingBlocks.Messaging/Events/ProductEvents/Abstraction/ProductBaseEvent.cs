namespace BuildingBlocks.Messaging.Events.ProductEvents.Abstraction;
public abstract record ProductBaseEvent : IntegrationEvent
{
    public Guid Id { get; set; }
    public string Sku { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public List<string> ImageFiles { get; set; }
    public int Quantity { get; set; }
    public string Color { get; set; }
    public bool IsActive { get; set; }
    public List<string> Categories { get; set; }
    public List<ProductAttribute> ProductAttributes { get; set; }
    public List<ProductReview> ProductReviews { get; set; }
}

public class ProductAttribute
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
}

public class ProductReview
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid CustomerId { get; set; } = default!;
    public string CustomerName { get; set; }
    public string Comment { get; set; } = default!;
    public int Rating { get; set; }
    public DateTime CreatedDate { get; set; }
}
