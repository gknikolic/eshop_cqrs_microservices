namespace Catalog.Write.Domain.ValueObjects;

public class Price
{
    public decimal Value { get; private set; }

    public Price(decimal value)
    {
        if (value <= 0)
            throw new DomainException("Price must be greater than zero");

        Value = value;
    }

    // Implicit conversion to decimal
    public static implicit operator decimal(Price self) => self.Value;
}
