using BuildingBlocks.DDD_Abstractions;

namespace Customer.API.Database.Entities;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Initials { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
}
