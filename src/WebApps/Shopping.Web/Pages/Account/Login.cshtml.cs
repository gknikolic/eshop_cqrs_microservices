using System.ComponentModel.DataAnnotations;
using Shopping.Web.Services.Clients;

namespace Shopping.Web.Pages.Account
{
    public class LoginModel(ICustomerService customerService, ILogger<LoginModel> logger, IAuthService authService)
        : PageModel
    {
      
        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        [BindProperty]
        public bool RememberMe { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await customerService.Login(new Models.Account.LoginRequestDto(Email, Password));

            if (response.token == null)
            {
                TempData["Error"] = "Invalid login attempt.";
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }

            await authService.SignInAsync(response.token, response.refreshToken);

            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            else
            {
                return RedirectToPage("/Index");
            }
        }

    }
}
