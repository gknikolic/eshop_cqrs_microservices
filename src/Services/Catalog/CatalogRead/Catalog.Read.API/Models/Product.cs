
namespace Catalog.Read.API.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<string> Categories { get; set; } = new();
    public string Description { get; set; } = default!;
    public string ImageFile { get; set; } = default!;
    public decimal Price { get; set; }

    public int PeicesInStock { get; set; }
    public bool IsActive { get; set; }

    public bool isAvailable => PeicesInStock > 0;
}