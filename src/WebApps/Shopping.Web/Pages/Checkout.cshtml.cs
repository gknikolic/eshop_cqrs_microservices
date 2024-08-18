using System.Text.Json;
using Shopping.Web.Helpers;
using Shopping.Web.Services.Clients;

namespace Shopping.Web.Pages
{
    public class CheckoutModel
        (IBasketService basketService, ILogger<CheckoutModel> logger)
        : PageModel
    {
        [BindProperty]
        public BasketCheckoutModel Order { get; set; } = default!;        
        public ShoppingCartModel Cart { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await basketService.LoadUserBasket(User);

            HttpContext.Session.SetString("OrderItems", JsonSerializer.Serialize(Cart.Items));

            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            logger.LogInformation("Checkout button clicked");

            Cart = await basketService.LoadUserBasket(User);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order.CustomerId = User.GetId();
            Order.UserName = Cart.UserName;
            Order.TotalPrice = Cart.TotalPrice;

            Order.Items = Cart.Items;

            //var orderItems = HttpContext.Session.GetString("OrderItems");
            //if (string.IsNullOrEmpty(orderItems))
            //{
            //    return BadRequest("Order items are not persisted in session.");
            //}

            //Order.Items = JsonSerializer.Deserialize<List<ShoppingCartItemModel>>(orderItems)!;

            await basketService.CheckoutBasket(new CheckoutBasketRequest(Order));

            return RedirectToPage("Confirmation", "OrderSubmitted");
        }
    }
}
