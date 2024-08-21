using Microsoft.AspNetCore.Mvc.Rendering;
using Shopping.Web.Services.Clients;

public class CreateProductModel(ICatalogService catalogService)
    : PageModel
{

    [BindProperty]
    public ProductViewModel Product { get; set; }

    public SelectList CategorySelectList { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        //if (!ModelState.IsValid)
        //{
        //    CategorySelectList = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
        //    return Page();
        //}

        //var product = new ProductDto(
        //    sku: new Sku(Product.Sku),
        //    name: Product.Name,
        //    description: Product.Description,
        //    price: new Price(Product.Price),
        //    categoryId: new CategoryId(Product.CategoryId)
        //)
        //{
        //    IsActive = Product.IsActive,
        //    Images = Product.Images.Split(',').Select((url, index) => new ProductImage(url.Trim(), url.Trim(), index + 1)).ToList()
        //};


        return RedirectToPage("./Index");
    }
}

public class ProductViewModel
{
    public string Sku { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public bool IsActive { get; set; }
    public string Images { get; set; } // Comma-separated URLs
}