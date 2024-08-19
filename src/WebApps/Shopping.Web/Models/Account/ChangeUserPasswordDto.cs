using System.ComponentModel.DataAnnotations;

namespace Shopping.Web.Models.Account;

public class ChangeUserPasswordDto
{
    [Required]
    public string UserId { get; set; }

    public string CurrentPassword { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Confirm password must be the same as Password")]
    public string ConfirmPassword { get; set; }
}
