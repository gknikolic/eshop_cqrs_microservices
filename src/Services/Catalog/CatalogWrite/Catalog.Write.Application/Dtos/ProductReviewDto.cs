namespace Catalog.Write.Application.Dtos;
public class ProductReviewDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
}
