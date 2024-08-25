using Catalog.Write.Domain.Models;
using Catalog.Write.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable(nameof(Customer));

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
               .HasConversion(id => id.Value, value => new CustomerId(value))
               .IsRequired();

        //builder.Property(c => c.Id)
        //       .ValueGeneratedNever();

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(c => c.Email)
               .IsRequired()
               .HasMaxLength(100);
    }
}