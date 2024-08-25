using Catalog.Write.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Catalog.Write.Domain.ValueObjects;

namespace Catalog.Write.Infrastructure.Data.Configurations;
public class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
{
    public void Configure(EntityTypeBuilder<ProductAttribute> builder)
    {
        builder.ToTable(nameof(ProductAttribute));

        builder.HasKey(pa => pa.Id);

        builder.Property(pa => pa.Id)
               .ValueGeneratedNever();

        builder.Property(pa => pa.Id).HasConversion(id => id.Value, value => new ProductAttributeId(value))
               .IsRequired();

        builder.Property(pa => pa.ProductId)
               .HasConversion(id => id.Value, value => new ProductId(value))
               .IsRequired();

        builder.Property(pa => pa.Key)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(pa => pa.Value)
               .IsRequired()
               .HasMaxLength(500);

        //builder.HasOne(pa => pa.Product)
        //       .WithMany(p => p.Attributes)
        //       .HasForeignKey(pa => pa.ProductId)
        //       .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pa => new { pa.ProductId, pa.Key }).IsUnique();
    }
}