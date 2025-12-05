using MahlineShop.Modules.Basket.Domain;
using MahlineShop.Modules.Basket.Persistence;
using MahlineShop.Shared.CQRS;
using MahlineShop.Shared.Result;
using MahlineShop.Shared.Services; // ⬅️ Our new service

namespace MahlineShop.Modules.Basket.Features.AddItemToBasket;

internal class AddItemToBasketHandler(
    IBasketRepository repository,
    ICurrentUser currentUser)
    : ICommandHandler<AddItemToBasketCommand, Result<decimal>>
{
    public async Task<Result<decimal>> Handle(AddItemToBasketCommand command, CancellationToken cancellationToken)
    {
        // 1. Get the User ID from the Token
        var userId = currentUser.UserId;

        // 2. Fetch existing basket from Redis
        var basket = await repository.GetBasketAsync(userId);

        // 3. If null, create a new one
        basket ??= new Domain.Basket { CustomerId = userId };

        // 4. Update Logic
        // Check if item already exists in cart
        var existingItem = basket.Items.FirstOrDefault(x => x.ProductId == command.ProductId);

        if (existingItem is not null)
        {
            // Since records are immutable, we replace the item
            // (Or if using classes, update quantity directly)
            var newItem = existingItem with { Quantity = existingItem.Quantity + command.Quantity };
            basket.Items.Remove(existingItem);
            basket.Items.Add(newItem);
        }
        else
        {
            // Add new item
            basket.Items.Add(new BasketItem
            {
                ProductId = command.ProductId,
                ProductName = command.ProductName,
                UnitPrice = command.UnitPrice,
                Quantity = command.Quantity
            });
        }

        // 5. Save back to Redis
        await repository.UpdateBasketAsync(basket);

        // 6. Return new Total Price
        return Result<decimal>.Success(basket.TotalPrice);
    }
}