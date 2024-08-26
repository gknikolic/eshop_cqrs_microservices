using Inventory.API.Dtos;

namespace Inventory.API.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public int Quantity { get; set; }

    internal InventoryItemDto ToInventoryItemDto()
    {
        return new InventoryItemDto(Id, Quantity);
    }
}
