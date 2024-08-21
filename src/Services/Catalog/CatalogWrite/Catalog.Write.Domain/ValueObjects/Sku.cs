namespace Catalog.Write.Domain.ValueObjects;

public class Sku
{
    public string Value { get; private set; }

    public Sku(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("SKU cannot be empty");

        Value = value;
    }

    // Implicit conversion to string
    public static implicit operator string(Sku self) => self.Value;
}
