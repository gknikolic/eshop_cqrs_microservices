using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Services.Clients;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Web.Pages.Account
{
    public class RegisterModel(ICustomerService customerService)
        : PageModel
    {

        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        [BindProperty]
        [Required]
        public string FirstName { get; set; }

        [BindProperty]
        [Required]
        public string LastName { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await customerService.Register(new Models.Account.RegisterRequestDto(Username, Email, Password, FirstName, LastName));

            if (response == null)
            {
                TempData["Error"] = "Invalid registration attempt.";
                ModelState.AddModelError(string.Empty, "Invalid registration attempt.");
                return Page();
            }

            return RedirectToPage("/Index");
        }
    }
}
