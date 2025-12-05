using MahlineShop.Shared.CQRS;
using MahlineShop.Shared.Result;

namespace MahlineShop.Modules.Basket.Features.AddItemToBasket;

// We return the updated total price of the basket
public record AddItemToBasketCommand(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity) : ICommand<Result<decimal>>;