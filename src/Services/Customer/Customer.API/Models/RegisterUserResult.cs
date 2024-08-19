using Customer.API.Database.Entities;

namespace Customer.API.Models;

public class RegisterUserResult
{
    public bool IsRegistered { get; set; } = false;
    public string? Message { get; set; }
    public User? User { get; set; }
}
