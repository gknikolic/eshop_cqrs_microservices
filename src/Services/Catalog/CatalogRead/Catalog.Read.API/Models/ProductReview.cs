namespace Catalog.Read.API.Models;

public class ProductReview
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Comment { get; set; } = default!;
    public int Rating { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CustomerName { get; set; } = default!;
    public Guid CustomerId { get; set; }
}
