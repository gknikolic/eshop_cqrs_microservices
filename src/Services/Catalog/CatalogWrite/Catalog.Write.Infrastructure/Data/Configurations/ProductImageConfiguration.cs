using Catalog.Write.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Catalog.Write.Domain.ValueObjects;

namespace Catalog.Write.Infrastructure.Data.Configurations;
public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable(nameof(ProductImage));

        builder.HasKey(pi => pi.Id);
        builder.Property(x => x.Id).HasConversion(
            id => id.Value,
            value => new ProductImageId(value));

        builder.Property(pi => pi.FilePath)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(pi => pi.AltText)
               .HasMaxLength(100);

        builder.Property(pi => pi.DisplayOrder)
               .IsRequired();

        builder.HasOne(pi => pi.Product)
               .WithMany(p => p.Images)
               .HasForeignKey(pi => pi.ProductId);
    }
}