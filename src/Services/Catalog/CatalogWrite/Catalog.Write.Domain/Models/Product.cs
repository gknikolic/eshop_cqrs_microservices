using System;

namespace Catalog.Write.Domain.Models;
public class Product : Aggregate<Guid>
{
    public Sku Sku { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Price Price { get; private set; }
    public bool IsActive { get; private set; }
    public Guid CategoryId { get; private set; }
    public virtual Category Category { get ; private set ; }
    public virtual List<ProductImage> Images { get; private set; }
    public virtual List<ProductReview> Reviews { get; private set; }
    public virtual List<ProductAttribute> Attributes { get; private set; }
    public Color Color { get; private set; }
    public Stock Stock { get; private set; }

    // Fields for versioning
    public int Version { get; private set; }
    public Guid? PreviousVersionId { get; private set; }
    public virtual Product PreviousVersion { get; private set; }

    // Private constructor for EF Core
    protected Product() { }

    public Product(Guid id, Sku sku, string name, string description, Price price, Color color, int version = 1, Guid? previousVersionId = null)
    {
        Id = id;
        Sku = sku;
        Name = name;
        Description = description;
        Price = price;
        IsActive = true;
        Color = color;
        Version = version;
        PreviousVersionId = previousVersionId;
        Stock = new Stock(0); // stock can be updated later with events from inventory service
        Images = new List<ProductImage>();
        Reviews = new List<ProductReview>();
        Attributes = new List<ProductAttribute>();
    }

    public Product CreateNewVersion()
    {
        var newProduct = new Product(
            id: Guid.NewGuid(),
            this.Sku,
            this.Name,
            this.Description,
            this.Price,
            this.Color,
            version: this.Version + 1,
            previousVersionId: this.Id
        );

        // copy inventory related props
        newProduct.Stock = this.Stock;

        this.IsActive = false;

        // 
        newProduct.ChangeCategory(this.Category);

        // Clone images
        foreach (var image in this.Images)
        {
            newProduct.AddImage(image.FilePath, image.AltText, image.DisplayOrder);
        }

        // Clone attributes
        foreach (var attribute in this.Attributes)
        {
            newProduct.AddAttribute(attribute.Key, attribute.Value);
        }

        // Clone reviews
        foreach (var review in this.Reviews)
        {
            newProduct.AddReview(new ProductReview(review.Rating, review.Comment, review.Customer));
        }

        return newProduct;
    }

    public void UpdateDetails(string name, string description, Price price, Guid categoryId, Color color)
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