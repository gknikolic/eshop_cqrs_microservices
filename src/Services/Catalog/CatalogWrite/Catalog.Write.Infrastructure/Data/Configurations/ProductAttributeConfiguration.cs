using Catalog.Write.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Write.Infrastructure.Data.Configurations;
public class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
{
    public void Configure(EntityTypeBuilder<ProductAttribute> builder)
    {
        builder.HasKey(pa => pa.Id);

        builder.Property(pa => pa.Id)
               .ValueGeneratedNever();

        builder.Property(pa => pa.Key)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(pa => pa.Value)
               .IsRequired()
               .HasMaxLength(500);

        builder.HasOne(pa => pa.Product)
               .WithMany(p => p.Attributes)
               .HasForeignKey(pa => pa.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pa => new { pa.ProductId, pa.Key }).IsUnique();
    }
}