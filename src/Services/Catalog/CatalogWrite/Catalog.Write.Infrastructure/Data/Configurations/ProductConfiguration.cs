using Catalog.Write.Domain.Enum;
using Catalog.Write.Domain.Models;
using Catalog.Write.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;


namespace Catalog.Write.Infrastructure.Data.Configurations;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(nameof(Product));

        builder.HasKey(p => p.Id);

        //builder.Property(p => p.Id)
        //       .HasConversion(id => id.Value, value => new ProductId(value))
        //       .IsRequired();

        //builder.Property(p => p.CategoryId).HasConversion(x => x.Value, p => new CategoryId(p));

        builder.Property(p => p.Version)
        .IsRequired()
        .HasDefaultValue(1);

       builder.HasOne(p => p.PreviousVersion)
            .WithOne()
            .HasForeignKey<Product>(p => p.PreviousVersionId);

        builder.Property(p => p.Sku)
               .HasConversion(sku => sku.Value, value => new Sku(value))
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.Description)
               .HasMaxLength(1000);

        builder.Property(p => p.Price)
               .HasConversion(price => price.Value, value => new Price(value))
               .IsRequired();

        builder.Property(p => p.Stock)
               .HasConversion(stock => stock.Quantity, value => new Stock(value))
               .IsRequired();

        builder.Property(p => p.Color).HasConversion(
            color => color.ToString(),
            value => (Color)Enum.Parse(typeof(Color), value))
                    .IsRequired();

        builder.HasMany(p => p.Images)
               .WithOne(i => i.Product)
               .HasForeignKey(i => i.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Reviews)
               .WithOne(r => r.Product)
               .HasForeignKey(r => r.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Attributes)
               .WithOne(a => a.Product)
               .HasForeignKey(a => a.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
