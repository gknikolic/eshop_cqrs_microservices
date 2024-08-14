namespace Shopping.Web.Models.Inventory;

public class UpdateInventoryItemModel
{
    public Guid Id { get; set; } = default!;
    public int Quantity { get; set; } = default!;
    public bool IsActive { get; set; } = default!;
}

// wrapper classes
public record UpdateInventoryItemResponse(InventoryItemModel Item);