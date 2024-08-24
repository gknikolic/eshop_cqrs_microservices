using Shopping.Web.Services.Clients;
using Microsoft.AspNetCore.Authorization;
using Shopping.Web.Helpers;

namespace Shopping.Web.Pages
{
    public class ProductDetailModel : PageModel
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly ILogger<ProductDetailModel> _logger;

        public ProductDetailModel(ICatalogService catalogService, IBasketService basketService, ILogger<ProductDetailModel> logger)
        {
            _catalogService = catalogService;
            _basketService = basketService;
            _logger = logger;
        }

        public ProductModel Product { get; set; } = default!;

        [BindProperty]
        public string Color { get; set; } = default!;

        [BindProperty]
        public int Quantity { get; set; } = default!;

        [BindProperty]
        public int Rating { get; set; } = default!;

        [BindProperty]
        public string Comment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid productId)
        {
            var response = await _catalogService.GetProduct(productId);
            Product = response.Product;

            return Page();
        }

        [Authorize]
        public async Task<IActionResult> OnPostAddReviewAsync(Guid productId)
        {
            if (Rating < 1 || Rating > 5 || string.IsNullOrWhiteSpace(Comment))
            {
                ModelState.AddModelError(string.Empty, "Invalid review input.");
                return Page();
            }

            var userName = User.Identity?.Name ?? "Anonymous";

            var review = new ProductReview
            {
                UserName = userName,
                Rating = Rating,
                Comment = Comment,
                UserId = User.GetId(),
                ProductId = productId
            };

            var result = await _catalogService.ReviewProduct(new ReviewProductRequest(review));

            return RedirectToPage("ProductDetail", new { productId });
        }

        [Authorize]
        public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
        {
            _logger.LogInformation("Add to cart button clicked");

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { returnUrl = $"/ProductDetail?productId={productId}" });
            }

            var productResponse = await _catalogService.GetProduct(productId);

            var basket = await _basketService.LoadUserBasket(User);

            basket.Items.Add(new ShoppingCartItemModel
            {
                ProductId = productId,
                ProductName = productResponse.Product.Name,
                Price = productResponse.Product.Price,
                Quantity = Quantity,
                Color = Color
            });

            await _basketService.StoreBasket(new StoreBasketRequest(basket));

            return RedirectToPage("Cart");
        }
    }
}
