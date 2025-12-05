using MahlineShop.Modules.Basket.Persistence;
using MahlineShop.Shared.Integration;

namespace MahlineShop.Modules.Basket.Integration;

internal class BasketIntegrationService(IBasketRepository repository) : IBasketIntegrationService
{
    public async Task<CustomerBasketDto?> GetBasketAsync(string customerId)
    {
        var basket = await repository.GetBasketAsync(customerId);
        if (basket is null) return null;

        // Map internal Domain to Shared DTO
        var items = basket.Items
            .Select(i => new BasketItemDto(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity))
            .ToList();

        return new CustomerBasketDto(basket.CustomerId, items);
    }

    public async Task ClearBasketAsync(string customerId)
    {
        await repository.DeleteBasketAsync(customerId);
    }
}