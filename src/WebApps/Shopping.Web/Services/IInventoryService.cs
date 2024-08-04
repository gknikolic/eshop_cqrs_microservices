﻿using Shopping.Web.Models.Inventory;

namespace Shopping.Web.Services;

public interface IInventoryService
{
    [Get("/inventory-service/inventory")]
    Task<GetInventoryResponse> GetInventory();
}
