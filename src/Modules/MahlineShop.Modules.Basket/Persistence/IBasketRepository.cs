namespace MahlineShop.Modules.Basket.Persistence;

public interface IBasketRepository
{
    Task<MahlineShop.Modules.Basket.Domain.Basket?> GetBasketAsync(string customerId);
    Task<bool> UpdateBasketAsync(MahlineShop.Modules.Basket.Domain.Basket basket);
    Task<bool> DeleteBasketAsync(string customerId);
}