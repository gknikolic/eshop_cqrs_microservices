using System.Text.Json;

namespace Shopping.Web.Pages
{
    public class CartModel(IBasketService basketService, ILogger<CartModel> logger)
        : PageModel
    {
        public ShoppingCartModel Cart { get; set; } = new ShoppingCartModel();

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await basketService.LoadUserBasket();

            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(Cart));

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(Guid productId)
        {
            logger.LogInformation("Remove to cart button clicked");
            Cart = await basketService.LoadUserBasket();

            Cart.Items.RemoveAll(x => x.ProductId == productId);

            await basketService.StoreBasket(new StoreBasketRequest(Cart));

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateQuantityAsync(Guid productId, int quantity)
        {
            // Retrieve cart from session
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new JsonResult(new { success = false });
            }

            Cart = JsonSerializer.Deserialize<ShoppingCartModel>(cartJson)!;

            // Update the quantity in the cart
            var cartItem = Cart.Items.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;

                await basketService.StoreBasket(new StoreBasketRequest(Cart));

                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(Cart));

                // Return the updated cart as JSON
                return new JsonResult(new { success = true, cart = Cart });
            }

            return new JsonResult(new { success = false });
        }
    }
}
