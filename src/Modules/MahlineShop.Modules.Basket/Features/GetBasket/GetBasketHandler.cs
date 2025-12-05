using MahlineShop.Modules.Basket.Persistence;
using MahlineShop.Shared.CQRS;
using MahlineShop.Shared.Result;
using MahlineShop.Shared.Services;

namespace MahlineShop.Modules.Basket.Features.GetBasket;

internal class GetBasketHandler(
    IBasketRepository repository,
    ICurrentUser currentUser)
    : IQueryHandler<GetBasketQuery, Result<MahlineShop.Modules.Basket.Domain.Basket>>
{
    public async Task<Result<MahlineShop.Modules.Basket.Domain.Basket>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        // 1. Get User ID from Token
        var userId = currentUser.UserId;

        // 2. Try to get basket from Redis
        var basket = await repository.GetBasketAsync(userId);

        // 3. Handle Empty State
        // If null, return a new empty basket so the frontend receives { items: [], totalPrice: 0 }
        basket ??= new MahlineShop.Modules.Basket.Domain.Basket { CustomerId = userId };

        return Result<MahlineShop.Modules.Basket.Domain.Basket>.Success(basket);
    }
}