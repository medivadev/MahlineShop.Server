using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;

namespace MahlineShop.Modules.Basket.Persistence;

internal class RedisBasketRepository(IDistributedCache cache) : IBasketRepository
{
    private readonly DistributedCacheEntryOptions _options = new()
    {
        // Baskets expire after 10 days of inactivity
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(10)
    };

    public async Task<MahlineShop.Modules.Basket.Domain.Basket?> GetBasketAsync(string customerId)
    {
        var data = await cache.GetAsync(customerId);
        if (data is null)
        {
            return null;
        }

        var basketJson = Encoding.UTF8.GetString(data);
        return JsonSerializer.Deserialize<MahlineShop.Modules.Basket.Domain.Basket>(basketJson);
    }

    public async Task<bool> UpdateBasketAsync(MahlineShop.Modules.Basket.Domain.Basket basket)
    {
        var basketJson = JsonSerializer.Serialize(basket);
        var data = Encoding.UTF8.GetBytes(basketJson);

        await cache.SetAsync(basket.CustomerId, data, _options);
        return true;
    }

    public Task<bool> DeleteBasketAsync(string customerId)
    {
        cache.Remove(customerId);
        return Task.FromResult(true);
    }
}