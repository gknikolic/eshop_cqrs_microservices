namespace Catalog.Write.Application.Dtos;
public class ProductReviewDto
{
    public Guid ProductId { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public DateTime Created { get; set; }
}
