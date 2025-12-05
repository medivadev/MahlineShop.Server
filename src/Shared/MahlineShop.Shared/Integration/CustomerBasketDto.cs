namespace MahlineShop.Shared.Integration;

public record CustomerBasketDto(string CustomerId, List<BasketItemDto> Items);

public record BasketItemDto(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity);