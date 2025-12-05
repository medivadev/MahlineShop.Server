using MahlineShop.Shared.CQRS;
using MahlineShop.Shared.Result;

namespace MahlineShop.Modules.Basket.Features.GetBasket;

// We return the Domain Entity directly here because it is a simple POCO (Plain Old CLR Object).
// In complex scenarios, map this to a specific BasketDto.
public record GetBasketQuery() : IQuery<Result<MahlineShop.Modules.Basket.Domain.Basket>>;