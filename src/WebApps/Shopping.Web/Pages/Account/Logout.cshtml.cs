using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shopping.Web.Pages.Account
{
    public class LogoutModel(ITokenProvider tokenProvider)
        : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            await HttpContext.SignOutAsync();
            tokenProvider.ClearTokens();
            return RedirectToPage("/Index");
        }
    }
}
