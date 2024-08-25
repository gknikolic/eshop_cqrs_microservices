using Catalog.Write.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Catalog.Write.Domain.ValueObjects;

namespace Catalog.Write.Infrastructure.Data.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(nameof(Category));
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
               .HasConversion(id => id.Value, value => new CategoryId(value))
               .IsRequired();

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(c => c.Description)
               .HasMaxLength(500);

        builder.Property(c => c.IsActive)
               .IsRequired();

        //builder.HasOne(c => c.ParentCategory)
        //       .WithMany(pc => pc.Subcategories)
        //       .HasForeignKey(c => c.ParentCategoryId)
        //       .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Products)
               .WithOne(p => p.Category)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        //builder.HasMany(c => c.Subcategories)
        //       .WithOne(x => x.ParentCategory)
        //       .HasForeignKey(c => c.ParentCategoryId)
        //       .OnDelete(DeleteBehavior.Restrict);

        //builder.Navigation(c => c.Subcategories)
        //       .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}