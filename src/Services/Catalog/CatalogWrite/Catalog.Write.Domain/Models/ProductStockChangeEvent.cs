namespace Catalog.Write.Domain.Models;
public class ProductStockChangeEvent : Entity<Guid>
{
    public ProductId ProductId { get; private set; }
    public int Quantity { get; private set; }
    public StockChangeReason Reason { get; private set; }

    public ProductStockChangeEvent(ProductId productId, int quantity, StockChangeReason reason)
    {
        ProductId = productId;
        Quantity = quantity;
        Reason = reason;
    }
}
