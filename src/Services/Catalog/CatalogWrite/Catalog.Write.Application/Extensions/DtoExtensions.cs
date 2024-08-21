namespace Catalog.Write.Application.Extensions;
public static class DtoExtensions
{
    // public static ProductDto ToDto(this Product product)
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Sku = product.Sku,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            PictureFileName = product.Images.FirstOrDefault()?.AltText ?? "",
            PictureUri = product.Images.FirstOrDefault()?.Url ?? "",
            Quantity = product.Stock.Quantity,
            IsAvailable = product.Stock.IsAvailable(),
            Color = product.Color.ToString(),
            CategoryId = product.CategoryId,
            Category = product.Category?.Name,
            Attributes = product.Attributes.Select(a => a.ToDto()).ToList(),
            Reviews = product.Reviews.Select(r => r.ToDto()).ToList()
        };
    }

    // public static ProductAttributeDto ToDto(this ProductAttribute attribute)
    public static ProductAttributeDto ToDto(this ProductAttribute attribute)
    {
        return new ProductAttributeDto
        {
            Id = attribute.Id,
            Name = attribute.Key,
            Value = attribute.Value
        };
    }

    // public static ProductReviewDto ToDto(this ProductReview review)
    public static ProductReviewDto ToDto(this ProductReview review)
    {
        return new ProductReviewDto
        {
            ProductId = review.ProductId,
            Comment = review.Comment,
            Rating = review.Rating,
            Created = review.CreatedAt.Value,
            UserId = review.CustomerId,
            Email = review.Customer.Email
        };
    }
}
