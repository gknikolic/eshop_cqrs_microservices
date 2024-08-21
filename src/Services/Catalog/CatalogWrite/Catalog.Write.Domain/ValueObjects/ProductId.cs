namespace Catalog.Write.Domain.ValueObjects;
public class ProductId
{
    public Guid Value { get; private set; }

    public ProductId(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException("Product ID cannot be empty");

        Value = value;
    }

    // Implicit conversion to Guid
    public static implicit operator Guid(ProductId self) => self.Value;
}
