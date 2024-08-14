using Inventory.API.Dtos;

namespace Inventory.API.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsActive { get; set; }

    internal InventoryItemDto ToInventoryItemDto()
    {
        return new InventoryItemDto(Id, Quantity, IsActive);
    }
}
