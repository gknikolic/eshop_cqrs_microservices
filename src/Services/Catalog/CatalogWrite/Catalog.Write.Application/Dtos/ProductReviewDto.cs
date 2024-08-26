namespace Catalog.Write.Application.Dtos;
public class ProductReviewDto
{
    public Guid ProductId { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
}
