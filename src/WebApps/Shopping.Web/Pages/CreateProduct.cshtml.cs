using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Shopping.Web.Services.Clients;

public class CreateProductModel(ICatalogService catalogService)
    : PageModel
{

    [BindProperty]
    public ProductViewModel Product { get; set; }

    public SelectList CategorySelectList { get; set; } = new SelectList(new List<string> { "Electronics", "Clothing", "Books", "Home", "Smart Phone", "White Appliances", "Camera", "Home Kitchen" });
    public SelectList ColorSelectList { get; set; } = new SelectList(new List<string> { "Red", "Blue", "Green", "Yellow", "Silver", "Gray", "Pink", "White" });

    // Maximum allowed file size in bytes (e.g., 2MB)
    private const long MaxFileSize = 2 * 1024 * 1024;

    // Permitted file extensions
    private readonly string[] _permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Product.ImageFiles == null || Product.ImageFiles.Count == 0)
        {
            ModelState.AddModelError("images", "Please select at least one image to upload.");
            return Page();
        }

        // Handle image uploads
        if (Product.ImageFiles != null && Product.ImageFiles.Any())
        {
            Product.UploadedImages = new List<string>();

            foreach (var imageFile in Product.ImageFiles)
            {
                // Validate the file (you can reuse the validation logic from previous steps)
                var validationError = ValidateImage(imageFile);
                if (!string.IsNullOrEmpty(validationError))
                {
                    ModelState.AddModelError("Product.ImageFiles", validationError);
                    return Page();
                }

                // Generate a unique file name and save the file
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                Product.UploadedImages.Add(fileName);
            }
        }

        // Save product details (assume the repository handles saving the product and images)
        var product = new ProductModel
        {
            Sku = Product.Sku,
            Name = Product.Name,
            Description = Product.Description,
            Price = Product.Price,
            Category = new List<string> { Product.Category },
            IsActive = Product.IsActive,
            ImageFiles = new List<string>(),
            ProductAttributes = Product.ProductAttributes.Select(attr => new ProductAttribute { Name = attr.Name, Value = attr.Value }).ToList()
        };

        // Add variants, attributes, etc., to the product as needed
        foreach (var imageName in Product.UploadedImages)
        {
            product.ImageFiles.Add($"/{imageName}");
        }


        return RedirectToPage("ProductList"); // Or wherever you want to redirect after creation

    }

    private string ValidateImage(IFormFile imageFile)
    {
        // Add your image validation logic here
        var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };
        var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(ext) || !allowedExtensions.Contains(ext))
        {
            return "Invalid image file type. Only .jpg, .jpeg, .png, and .gif are allowed.";
        }

        if (imageFile.Length > 2 * 1024 * 1024) // Example: 2MB limit
        {
            return "Image file size exceeds the maximum allowed size of 2MB.";
        }

        return null;
    }
}



public class ProductViewModel
{
    public Guid Id { get; set; }
    public string Sku { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public bool IsActive { get; set; }
    public string Color { get; set; }
    public List<string> UploadedImages { get; set; }
    public List<ProductAttribute> ProductAttributes { get; set; }

    // Add this property for handling file uploads
    public List<IFormFile> ImageFiles { get; set; }
}