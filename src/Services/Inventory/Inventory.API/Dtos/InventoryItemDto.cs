namespace Inventory.API.Dtos;

public record InventoryItemDto(Guid Id, int Quantity, bool IsActive);
