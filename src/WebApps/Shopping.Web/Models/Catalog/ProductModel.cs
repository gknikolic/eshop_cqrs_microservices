namespace Shopping.Web.Models.Catalog;

public class ProductModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Sku { get; set; }
    public string Description { get; set; } = default!;
    public List<string> Categories { get; set; } = new();
    public List<string> ImageFiles { get; set; } = default!;
    public decimal Price { get; set; }
    public string Color { get; set; }
    public string RateString { get; set; }
    public int PeicesInStock { get; set; }
    public bool IsInStock => PeicesInStock > 0;
    public List<ProductAttribute> ProductAttributes { get; set; } = new();
    public List<ProductReview> ProductReviews { get; set; } = new();

}

public class ProductReview
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = default!;
    public string Comment { get; set; } = default!;
    public int Rating { get; set; } // Assuming rating is out of 5
    public DateTime? CreatedDate { get; set; }
}

//wrapper classes
public record GetProductsResponse(IEnumerable<ProductModel> Products);
public record GetProductByCategoryResponse(IEnumerable<ProductModel> Products);
public record GetProductByIdResponse(ProductModel Product);
