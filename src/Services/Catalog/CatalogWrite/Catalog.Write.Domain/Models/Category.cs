namespace Catalog.Write.Domain.Models;
public class Category : Entity<CategoryId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public CategoryId? ParentCategoryId { get; private set; }
    public Category ParentCategory { get; private set; }
    public bool IsActive { get; private set; }

    // Kolekcija podkategorija
    public List<Category> Subcategories { get; private set; }

    // Navigacija prema proizvodima u ovoj kategoriji
    public List<Product> Products { get; private set; }

    // Private constructor za EF Core
    private Category() { }

    public Category(string name, string description, CategoryId? parentCategoryId = null)
    {
        Id = new CategoryId(new Guid());
        Name = name;
        Description = description;
        ParentCategoryId = parentCategoryId;
        IsActive = true;
        Subcategories = new List<Category>();
        Products = new List<Product>();
    }

    public void UpdateDetails(string name, string description)
    {
        Name = name ?? throw new ArgumentException("Category name cannot be null", nameof(name));
        Description = description;
    }

    public void AddSubcategory(Category subcategory)
    {
        if (subcategory == null)
            throw new ArgumentNullException(nameof(subcategory));

        Subcategories.Add(subcategory);
    }

    public void RemoveSubcategory(Category subcategory)
    {
        if (subcategory == null)
            throw new ArgumentNullException(nameof(subcategory));

        Subcategories.Remove(subcategory);
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}
