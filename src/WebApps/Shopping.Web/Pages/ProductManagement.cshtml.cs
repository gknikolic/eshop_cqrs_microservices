using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Services.Clients;

namespace Shopping.Web.Pages
{
    public class ProductManagementModel(ICatalogService catalogService)
        : PageModel
    {
        public List<ProductModel> Products { get; set; }

        public async Task OnGetAsync()
        {
            Products = (await catalogService.GetProducts()).Products.ToList();
        }
    }
}
