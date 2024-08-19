using Shopping.Web.Enums;
using Shopping.Web.Models.Account;
using Shopping.Web.Services.Clients;

namespace Shopping.Web.Pages.Account
{
    public class CustomersModel(ICustomerService customerService, IAuthService authService)
        : PageModel
    {
        public List<CustomerDto> Customers { get; set; }

        [BindProperty]
        public ChangeUserPasswordDto ChangeUserPasswordModel { get; set; }

        public async Task OnGetAsync()
        {
            var response = await customerService.GetCustomers();
            Customers = response.Users;
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await customerService.ChangeUserPassword(ChangeUserPasswordModel);
            if (result != null)
            {
                return RedirectToPage();
            }

            ModelState.AddModelError(string.Empty, "An error occurred while changing the password.");
            return Page();
        }

        public async Task<IActionResult> OnPostAssignRoleAsync(string userId, RoleEnum role)
        {
            var result = await customerService.ChangeUserRole(new ChangeUserRoleRequest(userId, new List<string> { role.ToString() }));
            if (result != null)
            {
                return RedirectToPage();
            }

            ModelState.AddModelError(string.Empty, "An error occurred while assigning the role.");
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteUser(string userId)
        {
            var result = await customerService.DeleteUser(userId);
            if(result != null)
            {
                if(result.Succeeded)
                {
                    return RedirectToPage();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while deleting the user.");
                    return Page();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> OnPostLoginAsAsync(string userId)
        {
            var response = await customerService.ImpersonateUser(new ImpersonateUserRequestDto(userId));

            if (response.token == null)
            {
                TempData["Error"] = "Invalid login attempt.";
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }

            await authService.SignInAsync(response.token, response.refreshToken);

            //return new JsonResult(new { success = true });

            return RedirectToPage("/Index");
        }
    }
}
