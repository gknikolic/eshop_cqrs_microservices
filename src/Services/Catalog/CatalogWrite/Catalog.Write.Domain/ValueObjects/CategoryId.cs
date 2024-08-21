namespace Catalog.Write.Domain.ValueObjects;

public class CategoryId
{
    public Guid Value { get; private set; }

    public CategoryId(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException("Category ID cannot be empty");

        Value = value;
    }

    // Implicit conversion to Guid
    public static implicit operator Guid(CategoryId self) => self.Value;
}