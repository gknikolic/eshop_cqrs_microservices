using Microsoft.AspNetCore.Mvc.Rendering;
using Shopping.Web.Enums;
using Shopping.Web.Services.Clients;
using System.ComponentModel.DataAnnotations;

public class CreateProductModel(ICatalogService catalogService)
    : PageModel
{

    [BindProperty]
    public ProductViewModel Product { get; set; }

    public SelectList CategorySelectList { get; set; } = new SelectList(new List<string> { "Electronics", "Clothing", "Books", "Home", "Smart Phone", "White Appliances", "Camera", "Home Kitchen" });
    public List<SelectListItem> ColorSelectList { get; set; } = Enum.GetValues(typeof(Color))
                                               .Cast<Color>()
                                                   .Select(e => new SelectListItem
                                                   {
                                                       Value = e.ToString(),
                                                       Text = e.ToString()
                                                   })
                                                   .ToList();
    public bool IsEditMode => Product.Id != Guid.Empty;

    // Maximum allowed file size in bytes (e.g., 2MB)
    private const long MaxFileSize = 2 * 1024 * 1024;

    // Permitted file extensions
    private readonly string[] _permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

    public async Task<IActionResult> OnGetAsync(Guid? Id)
    {
        if(Id.HasValue)
        {
            var response = await catalogService.GetProduct(Id.Value);
            var product = response.Product;
            Product = new ProductViewModel
            {
                Id = product.Id,
                Sku = product.Sku,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Categories.FirstOrDefault(),
                Color = product.Color.ToString(),
                Rate = product.RateString,
                UploadedImages = product.ImageFiles.Select(image => Path.GetFileName(image)).ToList(),
                ProductAttributes = product.ProductAttributes.Select(attr => new ProductAttribute { Name = attr.Name, Value = attr.Value }).ToList()
            };
        }
        else
        {
            Product = new ProductViewModel();
        }
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
                var fileName = $"{Guid.NewGuid().ToString().Replace("-", "")}_$_{Path.GetFileName(imageFile.FileName.Split("_$_").Last())}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/product", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                Product.UploadedImages.Add(fileName);
            }
        }

        // Save product details (assume the repository handles saving the product and images)
        var product = new CreateUpdateProductModel
        {
            Sku = Product.Sku,
            Name = Product.Name,
            Description = Product.Description,
            Price = Product.Price.Value,
            Category = Product.Category,
            Images = Product.UploadedImages.Select(x=> new ProductImageModel { FilePath = x, AltText = x, DisplayOrder = 1}).ToList(),
            Color = Product.Color,
            Attributes = Product.ProductAttributes.Select(attr => new ProductAttribute { Name = attr.Name, Value = attr.Value }).ToList(),
        };

        var imageResult = await HandleImages();

        if (imageResult.success == false)
        {
            ModelState.AddModelError("Product.ImageFiles", imageResult.errors);
            return Page();
        }

        if (IsEditMode)
        {
            product.Id = Product.Id;
            var result = await catalogService.UpdateProduct(new UpdateProductRequest(product));
            return RedirectToPage("ProductList");

        }
        else
        {
            var result = await catalogService.CreateProduct(new CreateProductRequest(product));
            return RedirectToPage("ProductList");
        }

    }

    private async Task<(bool success, string errors)> HandleImages()
    {
        // Handle image uploads
        Product.UploadedImages = new List<string>();

        foreach (var imageFile in Product.ImageFiles)
        {
            var validationError = ValidateImage(imageFile);
            if (!string.IsNullOrEmpty(validationError))
            {
                return (false, validationError);
            }

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            Product.UploadedImages.Add(fileName);
        }

        return (true, "");
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
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Sku { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    [Required]
    public decimal? Price { get; set; }
    [Required]
    public string Category { get; set; }
    public bool IsActive { get; set; }
    public string Color { get; set; }
    public string? Rate { get; set; }
    public List<string>? UploadedImages { get; set; }
    public List<ProductAttribute> ProductAttributes { get; set; }

    [Required]
    public List<IFormFile> ImageFiles { get; set; } // this property for handling file uploads
}