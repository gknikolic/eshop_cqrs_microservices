namespace Catalog.Write.Domain.Models;
public class Product : Aggregate<ProductId>
{
    public Sku Sku { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Price Price { get; private set; }
    public bool IsActive { get; private set; }
    public CategoryId CategoryId { get; private set; }
    public virtual Category Category { get ; private set ; }
    public virtual List<ProductImage> Images { get; private set; }
    public virtual List<ProductReview> Reviews { get; private set; }
    public virtual List<ProductAttribute> Attributes { get; private set; }
    public Color Color { get; private set; }
    public Stock Stock { get; private set; }

    // Private constructor for EF Core
    private Product() { }

    public Product(Guid id, Sku sku, string name, string description, Price price, Color color)
    {
        Id = new ProductId(id);
        Sku = sku;
        Name = name;
        Description = description;
        Price = price;
        IsActive = true;
        Color = color;
        Stock = new Stock(0); // stock can be updated later with events from inventory service
        Images = new List<ProductImage>();
        Reviews = new List<ProductReview>();
        Attributes = new List<ProductAttribute>();
    }

    public void UpdateDetails(string name, string description, Price price, CategoryId categoryId, Color color)
    {
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
        Color = color;
    }

    public void UpdateStock(int quantity)
    {
        Stock = new Stock(quantity);
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void AddImage(string url, string altText, int displayOrder)
    {
        var image = new ProductImage(url, altText, displayOrder);
        Images.Add(image);
    }

    public void RemoveImage(ProductImage image)
    {
        if (Images.Contains(image))
        {
            Images.Remove(image);
        }
    }

    public void AddReview(ProductReview review)
    {
        Reviews.Add(review);
    }

    public void RemoveReview(Guid reviewId)
    {
        if(Reviews.Any(r => r.Id == reviewId))
        {
            var review = Reviews.Single(r => r.Id == reviewId);
            Reviews.Remove(review);
        }
    }

    public void ChangeCategory(Category category)
    {
        Category = category ?? throw new ArgumentNullException(nameof(category));
        CategoryId = category.Id;
    }

    public void AddOrUpdateAttribute(string key, string value)
    {
        var attribute = Attributes.SingleOrDefault(a => a.Key == key);
        if (attribute != null)
        {
            attribute.UpdateValue(value);
        }
        else
        {
            AddAttribute(key, value);
        }
    }

    public void AddAttribute(string key, string value)
    {
        var attribute = new ProductAttribute(key, value, Id);
        Attributes.Add(attribute);
    }

    public void UpdateAttribute(string key, string value)
    {
        var attribute = Attributes.SingleOrDefault(a => a.Key == key);
        if (attribute != null)
        {
            attribute.UpdateValue(value);
        }
    }

    public void RemoveAttribute(string key)
    {
        var attribute = Attributes.SingleOrDefault(a => a.Key == key);
        if (attribute != null)
        {
            Attributes.Remove(attribute);
        }
    }

}