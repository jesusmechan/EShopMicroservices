namespace Basket.API.Data;
public class CachedBasketRepository
    (IBasketRepository repository, IDistributedCache cache)
    : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var cacheBasket = await cache.GetStringAsync(userName, cancellationToken);
        if(!string.IsNullOrEmpty(cacheBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cacheBasket) ?? throw new BasketNotFoundException(userName);
        
        var basket = await repository.GetBasket(userName, cancellationToken);
        
        // Guardar en cache para futuras consultas
        var jsonBasket = JsonSerializer.Serialize(basket);
        await cache.SetStringAsync(userName, jsonBasket, cancellationToken);
        
        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        var result = await repository.StoreBasket(basket, cancellationToken);
        await cache.SetStringAsync(result.UserName, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;

    }
    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        var result = await repository.DeleteBasket(userName, cancellationToken);
        await cache.RemoveAsync(userName, cancellationToken);
        return true;
    }
}
