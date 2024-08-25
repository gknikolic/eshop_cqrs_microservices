namespace Catalog.Write.Domain.Models;
public class ProductAttribute : Entity<ProductAttributeId>
{
    public string Key { get; private set; }
    public string Value { get; private set; }
    public ProductId ProductId { get; private set; }
    public virtual Product Product { get; private set; }

    // Private constructor for EF Core
    protected ProductAttribute() { }

    public ProductAttribute(string key, string value, ProductId productId)
    {
        Id = new ProductAttributeId(Guid.NewGuid());
        Key = key ?? throw new ArgumentNullException(nameof(key));
        Value = value ?? throw new ArgumentNullException(nameof(value));
        ProductId = productId ?? throw new ArgumentNullException(nameof(productId));
    }

    public void UpdateValue(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }
}