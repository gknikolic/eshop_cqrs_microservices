namespace Catalog.Write.Domain.ValueObjects;

public class ProductReviewId
{
    public Guid Value { get; private set; }

    public ProductReviewId(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException("ProductReview ID cannot be empty");

        Value = value;
    }

    // Implicit conversion to Guid
    public static implicit operator Guid(ProductReviewId self) => self.Value;
}
