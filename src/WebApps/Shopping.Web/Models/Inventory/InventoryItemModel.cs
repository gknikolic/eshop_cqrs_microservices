namespace Shopping.Web.Models.Inventory;

public class InventoryItemModel
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public decimal Price { get; set; } = default!;
    public int Quantity { get; set; } = default!;
    public bool IsActive { get; set; } = default!;
}

// wrapper classes
public record GetInventoryResponse(List<InventoryItemModel> Products);