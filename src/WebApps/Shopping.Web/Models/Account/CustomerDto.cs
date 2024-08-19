namespace Shopping.Web.Models.Account;

public class CustomerDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public bool EmailConfirmed { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
    public string RolesString => string.Join(", ", Roles);
}
