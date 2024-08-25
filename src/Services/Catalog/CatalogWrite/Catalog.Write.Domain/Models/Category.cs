namespace Catalog.Write.Domain.Models;
public class Category : Entity<CategoryId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    //public virtual CategoryId? ParentCategoryId { get; private set; }
    //public virtual Category ParentCategory { get;  set; }
    public bool IsActive { get; private set; }

    //public virtual List<Category> Subcategories { get;  set; }

    public virtual List<Product> Products { get; private set; }

    // Private constructor za EF Core
    protected Category() { }

    public Category(string name, string description, CategoryId? parentCategoryId = null)
    {
        Id = new CategoryId(Guid.NewGuid());
        Name = name;
        Description = description;
        //ParentCategoryId = parentCategoryId;
        IsActive = true;
        //Subcategories = new List<Category>();
        Products = new List<Product>();
    }

    public void UpdateDetails(string name, string description)
    {
        Name = name ?? throw new ArgumentException("Category name cannot be null", nameof(name));
        Description = description;
    }

    //public void AddSubcategory(Category subcategory)
    //{
    //    if (subcategory == null)
    //        throw new ArgumentNullException(nameof(subcategory));

    //    Subcategories.Add(subcategory);
    //}

    //public void RemoveSubcategory(Category subcategory)
    //{
    //    if (subcategory == null)
    //        throw new ArgumentNullException(nameof(subcategory));

    //    Subcategories.Remove(subcategory);
    //}

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}
