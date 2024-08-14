using Shopping.Web.Models.Inventory;

namespace Shopping.Web.Pages;

public class InventoryModel(IInventoryService inventoryService, ILogger<InventoryModel> logger)
    : PageModel
{
    public IEnumerable<InventoryItemModel> Items { get; set; } = default!;

    [BindProperty]
    public UpdateInventoryItemModel UpdateInventoryItemModel { get; set; } = default!;

    public async Task<IActionResult> OnGet()
    {
        var response = await inventoryService.GetInventory();
        Items = response.Products;

        return Page();
    }

    public async Task<IActionResult> OnPostUpdateItemAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await inventoryService.UpdateInventoryItem(UpdateInventoryItemModel);
        if (result != null)
        {
            return RedirectToPage();
        }

        ModelState.AddModelError(string.Empty, "An error occurred while updating the item.");
        return Page();
    }
}