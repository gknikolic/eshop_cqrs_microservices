namespace Shopping.Web.Models.Catalog;

public class ProductModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Sku { get; set; }
    public List<string> Category { get; set; } = new();
    public string Description { get; set; } = default!;
    public List<string> ImageFiles { get; set; } = default!;
    public decimal Price { get; set; }
    public int PeicesInStock { get; set; }
    public bool IsActive { get; set; }
    public List<ProductAttribute> ProductAttributes { get; set; }
    public List<ProductReview> ProductReviews { get; set; } = new();

}

public class ProductReview
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = default!;
    public string Comment { get; set; } = default!;
    public int Rating { get; set; } // Assuming rating is out of 5
}

//wrapper classes
public record GetProductsResponse(IEnumerable<ProductModel> Products);
public record GetProductByCategoryResponse(IEnumerable<ProductModel> Products);
public record GetProductByIdResponse(ProductModel Product);
