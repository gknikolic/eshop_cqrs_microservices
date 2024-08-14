using System.ComponentModel.DataAnnotations;

namespace Shopping.Web.Models.Inventory;

public class InventoryItemDto
{
    public Guid Id { get; set; } = default!;

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a whole number greater than or equal to 0")]
    public int Quantity { get; set; } = default!;
    public bool IsActive { get; set; } = default!;
}

// wrapper classes
public record UpdateInventoryItemRequest(InventoryItemModel InventoryItemDto);
public record UpdateInventoryItemResponse(InventoryItemModel InventoryItemDto);
