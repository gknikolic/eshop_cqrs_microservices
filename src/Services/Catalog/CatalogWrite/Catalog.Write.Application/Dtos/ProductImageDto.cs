namespace Catalog.Write.Application.Dtos;
public class ProductImageDto
{
    public Guid ProductId { get; set; }
    public string FilePath { get; set; }
    public string? AltText { get; set; }
    public int DisplayOrder { get; set; }
}
