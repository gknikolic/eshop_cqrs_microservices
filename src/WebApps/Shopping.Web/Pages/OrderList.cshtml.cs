using Microsoft.AspNetCore.Authorization;
using Shopping.Web.Helpers;
using Shopping.Web.Services.Clients;

namespace Shopping.Web.Pages
{
    [Authorize]
    public class OrderListModel
        (IOrderingService orderingService, ILogger<OrderListModel> logger)
        : PageModel
    {
        public IEnumerable<OrderModel> Orders { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            // assumption customerId is passed in from the UI authenticated user swn
            var customerId = User.GetId();

            var response = await orderingService.GetOrdersByCustomer(customerId);
            Orders = response.Orders;

            return Page();
        }
    }
}
