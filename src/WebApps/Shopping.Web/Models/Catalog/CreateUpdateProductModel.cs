namespace Shopping.Web.Models.Catalog;

public class CreateUpdateProductModel
{
    public Guid Id { get; set; }
    public string Sku { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Color { get; set; }
    public string? Category { get; set; }
    public List<ProductAttribute> Attributes { get; set; }
    public List<ProductImageModel> Images { get; set; }
}
