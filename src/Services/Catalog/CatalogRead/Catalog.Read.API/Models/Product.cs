
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

    public List<ProductReview> Reviews { get; set; }
    public List<ProductAttribute> Attributes { get; set; }

    public bool isAvailable => PeicesInStock > 0;
    public double AverageRating => Reviews?.Count > 0 ? Reviews.Average(r => r.Rating) : 0;
}