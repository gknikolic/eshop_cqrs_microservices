namespace Catalog.Write.Domain.ValueObjects;

public class Stock
{
    public int Quantity { get; private set; }

    public Stock(int quantity)
    {
        if (quantity < 0)
            throw new DomainException("Stock quantity cannot be negative");

        Quantity = quantity;
    }

    public bool IsAvailable() => Quantity > 0;
}