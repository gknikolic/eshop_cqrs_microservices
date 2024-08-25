namespace Catalog.Write.Domain.Models;
public class Customer : Entity<Guid>
{
    public string Name { get; private set; }
    public string Email { get; private set; }

    // Private constructor for EF Core
    protected Customer() { }

    public Customer(Guid id , string name, string email)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }
}
