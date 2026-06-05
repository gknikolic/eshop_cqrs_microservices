namespace Catalog.Write.Domain.Models;
public class ProductStockChangeEvent : Entity<Guid>
{
    public virtual Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public virtual StockChangeReason Reason { get; private set; }

    public ProductStockChangeEvent(Guid productId, int quantity, StockChangeReason reason)
    {
        ProductId = productId;
        Quantity = quantity;
        Reason = reason;
    }
}
