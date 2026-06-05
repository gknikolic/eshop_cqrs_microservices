using Refit;
using Shopping.Web.Models.Inventory;

namespace Shopping.Web.Services.Clients;

public interface IInventoryService
{
    [Get("/inventory-service/inventory")]
    Task<GetInventoryResponse> GetInventory();

    [Put("/inventory-service/inventory")]
    Task<UpdateInventoryItemResponse> UpdateInventoryItem(UpdateInventoryItemRequest request);
}
