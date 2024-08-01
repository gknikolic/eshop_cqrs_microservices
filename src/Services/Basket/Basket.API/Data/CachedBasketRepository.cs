using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data;

public class CachedBasketRepository
    (IBasketRepository repository, IDistributedCache cache)
    : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

        var basket = await repository.GetBasket(userName, cancellationToken);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
        await cache.SetStringAsync(basket.UserName + "_itemCount", JsonSerializer.Serialize(basket.Items.Count), cancellationToken);
        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.StoreBasket(basket, cancellationToken);

        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);
        await cache.SetStringAsync(basket.UserName + "_itemCount", JsonSerializer.Serialize(basket.Items.Count), cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasket(userName, cancellationToken);

        await cache.RemoveAsync(userName, cancellationToken);
        await cache.RemoveAsync(userName + "_itemCount", cancellationToken);

        return true;
    }

    public async Task<int> GetBasketItemCount(string userName, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await cache.GetStringAsync(userName + "_itemCount", cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
            return JsonSerializer.Deserialize<int>(cachedBasket)!;

        var itemsCount = await repository.GetBasketItemCount(userName, cancellationToken);
        await cache.SetStringAsync(userName + "_itemCount", JsonSerializer.Serialize(itemsCount), cancellationToken);

        return itemsCount;
    }
}
