namespace Catalog.Write.Domain.ValueObjects;
public class ProductImageId
{
    public Guid Value { get; private set; }

    public ProductImageId(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException("ProductImage ID cannot be empty");

        Value = value;
    }

    // Implicit conversion to Guid
    public static implicit operator Guid(ProductImageId self) => self.Value;
}
