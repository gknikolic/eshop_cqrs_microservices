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
}

public class ProductAttribute
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
}
