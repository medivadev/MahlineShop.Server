using MahlineShop.Modules.Ordering.Data;
using MahlineShop.Modules.Ordering.Domain;
using MahlineShop.Shared.CQRS;
using MahlineShop.Shared.Integration; // ⬅️ Using the Shared Contract
using MahlineShop.Shared.Result;
using MahlineShop.Shared.Services;
using MediatR;

namespace MahlineShop.Modules.Ordering.Orders.Features.CheckoutOrder;

internal class CheckoutOrderHandler(
    OrderingDbContext dbContext,
    ICurrentUser currentUser,
    IBasketIntegrationService basketService,
    IPublisher publisher) // ⬅️ Injected
    : ICommandHandler<CheckoutOrderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CheckoutOrderCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;

        // 1. Get Basket Data (Cross-Module Call)
        var basket = await basketService.GetBasketAsync(userId);

        if (basket is null || basket.Items.Count == 0)
        {
            return Result<Guid>.Failure(new Error("Order.EmptyBasket", "Cannot checkout an empty basket."));
        }

        // 2. Create Order Aggregate
        var order = Order.Create(userId);

        // 3. Snapshot Pattern: Copy items from Basket to Order
        // We do NOT reference the Catalog here. We trust the Basket's data (or re-validate if needed).
        foreach (var item in basket.Items)
        {
            order.AddItem(item.ProductId, item.ProductName, item.UnitPrice, item.Quantity);
        }

        // 4. Persist Order
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        // 5. Cleanup: Clear the Basket now that the order is placed
        //await basketService.ClearBasketAsync(userId); ==> Instead of this we use an event!
        await publisher.Publish(new OrderCreatedEvent(order.Id, userId), cancellationToken);

        return Result<Guid>.Success(order.Id);
    }
}