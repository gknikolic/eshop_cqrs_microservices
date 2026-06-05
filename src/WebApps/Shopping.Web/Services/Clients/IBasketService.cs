using Refit;
using Shopping.Web.Helpers;
using System.Net;
using System.Security.Claims;

namespace Shopping.Web.Services.Clients;

public interface IBasketService
{
    [Get("/basket-service/basket/{userName}")]
    Task<GetBasketResponse> GetBasket(string userName);

    [Get("/basket-service/basket/{userName}/count")]
    Task<GetBasketItemCountResponse> GetBasketItemCount(string userName);

    [Post("/basket-service/basket")]
    Task<StoreBasketResponse> StoreBasket(StoreBasketRequest request);

    [Delete("/basket-service/basket/{userName}")]
    Task<DeleteBasketResponse> DeleteBasket(string userName);

    [Post("/basket-service/basket/checkout")]
    Task<CheckoutBasketResponse> CheckoutBasket(CheckoutBasketRequest request);


    public async Task<ShoppingCartModel> LoadUserBasket(ClaimsPrincipal user)
    {
        if (user == null || user.Identity.IsAuthenticated == false)
        {
            return new ShoppingCartModel
            {
                UserName = "guest",
                Items = []
            };
        }

        // Get Basket If Not Exist Create New Basket with Default Logged In User Name: swn
        var userName = user.GetEmail();
        ShoppingCartModel basket;

        try
        {
            var getBasketResponse = await GetBasket(userName);
            basket = getBasketResponse.Cart;
        }
        catch (ApiException apiException) when (apiException.StatusCode == HttpStatusCode.NotFound)
        {
            basket = new ShoppingCartModel
            {
                UserName = userName,
                Items = []
            };
        }

        return basket;
    }

    public async Task<int> LoadUserBasketItemsCount(ClaimsPrincipal user)
    {
        if (user == null || user.Identity.IsAuthenticated == false)
        {
            return 0;
        }

        // Get Basket If Not Exist Create New Basket with Default Logged In User Name: swn
        var userName = user.GetEmail();

        try
        {
            var response = await GetBasketItemCount(userName);
            return response.Count;
        }
        catch (ApiException apiException) when (apiException.StatusCode == HttpStatusCode.NotFound)
        {
            return 0;
        }
    }
}
