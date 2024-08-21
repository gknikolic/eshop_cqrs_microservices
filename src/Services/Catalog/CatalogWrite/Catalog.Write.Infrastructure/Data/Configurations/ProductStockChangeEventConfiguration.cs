using Catalog.Write.Domain.Enum;
using Catalog.Write.Domain.Models;
using Catalog.Write.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Write.Infrastructure.Data.Configurations;
public class ProductStockChangeEventConfiguration : IEntityTypeConfiguration<ProductStockChangeEvent>
{
    public void Configure(EntityTypeBuilder<ProductStockChangeEvent> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .ValueGeneratedNever();

        builder.Property(e => e.ProductId)
               .HasConversion(id => id.Value, value => new ProductId(value))
               .IsRequired();

        builder.Property(e => e.Quantity)
               .IsRequired();

        builder.Property(e => e.Reason)
               .IsRequired()
               .HasConversion(
                   reason => reason.ToString(),
                   value => (StockChangeReason)Enum.Parse(typeof(StockChangeReason), value)
               );

        builder.HasIndex(e => e.ProductId);
    }
}
