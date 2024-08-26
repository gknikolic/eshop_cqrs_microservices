
namespace Catalog.Read.API.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<string> Categories { get; set; } = new();
    public string Description { get; set; } = default!;
    public List<string> ImageFiles { get; set; } = default!;
    public decimal Price { get; set; }
    public string Color { get; set; }

    public int PeicesInStock { get; set; }
    public bool IsActive { get; set; }

    public string RateString => ProductReviews?.Count > 0 ? ProductReviews.Select(x => x.Rating).Average().ToString("0.##") + $" of {ProductReviews?.Count} rates." : "Not yet rated";

    public List<ProductReview> ProductReviews { get; set; }
    public List<ProductAttribute> ProductAttributes { get; set; }

    public bool isAvailable => PeicesInStock > 0;
    public double AverageRating => ProductReviews?.Count > 0 ? ProductReviews.Average(r => r.Rating) : 0;
}