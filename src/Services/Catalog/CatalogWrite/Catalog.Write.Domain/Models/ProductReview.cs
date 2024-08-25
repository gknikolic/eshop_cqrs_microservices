﻿namespace Catalog.Write.Domain.Models;
public class ProductReview : Entity<ProductReviewId>
{
    public int Rating { get; private set; }
    public string Comment { get; private set; }

    public virtual ProductId ProductId { get; private set; }
    public virtual Product Product { get; set; }
    public virtual CustomerId CustomerId { get; private set; }
    public virtual Customer Customer { get; set; }


    // for ef
    public ProductReview() { }

    public ProductReview(int rating, string comment, Customer customer)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5", nameof(rating));

        Id = new ProductReviewId(Guid.NewGuid());
        Rating = rating;
        Comment = comment ?? throw new ArgumentException("Comment cannot be null", nameof(comment));
        Customer = customer ?? throw new ArgumentException("Customer cannot be null", nameof(customer));
    }

    public void UpdateComment(string comment)
    {
        Comment = comment ?? throw new ArgumentException("Comment cannot be null", nameof(comment));
        // don't update CreatedAt as we don't want to change the review date
    }
}
