namespace Catalog.Write.Domain.Models;
public class ProductImage : Entity<ProductImageId>
{
    public string FilePath { get; private set; }
    public string AltText { get; private set; }
    public int DisplayOrder { get; private set; }

    // Navigacija prema parent entitetu
    public virtual ProductId ProductId { get; private set; }
    public virtual Product Product { get; private set; }

    // Private constructor za EF Core
    protected ProductImage() { }

    public ProductImage(string url, string altText, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Image URL cannot be empty", nameof(url));

        Id = new ProductImageId(Guid.NewGuid());
        FilePath = url;
        AltText = altText;
        DisplayOrder = displayOrder;
    }

    public void UpdateDetails(string url, string altText, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Image URL cannot be empty", nameof(url));

        FilePath = url;
        AltText = altText;
        DisplayOrder = displayOrder;
    }
}
