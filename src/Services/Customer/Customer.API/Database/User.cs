using Microsoft.AspNetCore.Identity;

namespace Customer.API.Database;

public class User : IdentityUser
{
    public string? Initials { get; set; }
}
