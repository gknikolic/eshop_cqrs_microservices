namespace Catalog.Write.Application.Dtos;
public class ProductDto
{
    public Guid Id { get; set; }
    public string Sku { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsAvailable { get; set; }
    public string Color { get; set; }
    public Guid CategoryId { get; set; }
    public string? Category { get; set; }
    public List<ProductAttributeDto> Attributes { get; set; }
    public List<ProductReviewDto> Reviews { get; set; }
    public List<ProductImageDto> Images { get; set; }

}
