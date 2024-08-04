using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Inventory;

namespace Shopping.Web.Pages;

public class InventoryModel(IInventoryService inventoryService, ILogger<OrderListModel> logger) 
    : PageModel
{
    public IEnumerable<InventoryItemModel> Items { get; set; } = default!;

    public async Task<IActionResult> OnGet()
    {
        var response = await inventoryService.GetInventory();
        Items = response.Products;

        return Page();
    }
}
