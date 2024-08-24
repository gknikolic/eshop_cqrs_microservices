using Catalog.Write.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Catalog.Write.Domain.ValueObjects;

namespace Catalog.Write.Infrastructure.Data.Configurations;
public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
{
    public void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        builder.ToTable(nameof(ProductReview));

        builder.HasKey(pr => pr.Id);

        builder.Property(pr => pr.Id)
               .HasConversion(id => id.Value, value => new ProductReviewId(value))
               .IsRequired();

        builder.Property(pr => pr.Rating)
               .IsRequired();

        builder.Property(pr => pr.Comment)
               .HasMaxLength(1000);

        builder.Property(pr => pr.CreatedAt)
               .IsRequired();

        builder.HasOne(pr => pr.Product)
               .WithMany(p => p.Reviews)
               .HasForeignKey(pr => pr.ProductId);

        builder.HasOne(pr => pr.Customer)
               .WithMany()
               .HasForeignKey(pr => pr.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
