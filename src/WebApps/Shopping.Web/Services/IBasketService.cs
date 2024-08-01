using System.Net;

namespace Shopping.Web.Services;

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


    public async Task<ShoppingCartModel> LoadUserBasket()
    {
        // Get Basket If Not Exist Create New Basket with Default Logged In User Name: swn
        var userName = "swn";
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

    public async Task<int> LoadUserBasketItemsCount()
    {
        // Get Basket If Not Exist Create New Basket with Default Logged In User Name: swn
        var userName = "swn";

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
