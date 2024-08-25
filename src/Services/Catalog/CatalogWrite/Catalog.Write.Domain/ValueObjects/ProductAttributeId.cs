namespace Catalog.Write.Domain.ValueObjects;

public class ProductAttributeId
{
    public Guid Value { get; private set; }

    public ProductAttributeId(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException("ProductAttribute ID cannot be empty");

        Value = value;
    }

    // Implicit conversion to Guid
    public static implicit operator Guid(ProductAttributeId self) => self.Value;
}
