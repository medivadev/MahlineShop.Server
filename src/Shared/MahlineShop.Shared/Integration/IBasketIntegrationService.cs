namespace MahlineShop.Shared.Integration;

public interface IBasketIntegrationService
{
    Task<CustomerBasketDto?> GetBasketAsync(string customerId);
    Task ClearBasketAsync(string customerId);
}