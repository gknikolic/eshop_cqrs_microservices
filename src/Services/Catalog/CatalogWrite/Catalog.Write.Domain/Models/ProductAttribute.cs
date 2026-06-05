namespace Catalog.Write.Domain.Models;
public class ProductAttribute : Entity<Guid>
{
    public string Key { get; private set; }
    public string Value { get; private set; }
    public Guid ProductId { get; private set; }
    public virtual Product Product { get; private set; }

    // Private constructor for EF Core
    protected ProductAttribute() { }

    public ProductAttribute(string key, string value, Guid productId)
    {
        Id = Guid.NewGuid();
        Key = key ?? throw new ArgumentNullException(nameof(key));
        Value = value ?? throw new ArgumentNullException(nameof(value));
        ProductId = productId;
    }

    public void UpdateValue(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }
}