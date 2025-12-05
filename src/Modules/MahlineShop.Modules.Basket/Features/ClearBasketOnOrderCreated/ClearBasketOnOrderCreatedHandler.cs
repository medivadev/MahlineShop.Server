using MahlineShop.Modules.Basket.Persistence;
using MahlineShop.Shared.Integration;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MahlineShop.Modules.Basket.Features.ClearBasketOnOrderCreated;

internal class ClearBasketHandler(
    IBasketRepository repository,
    ILogger<ClearBasketHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Clearing basket for customer {CustomerId} after Order {OrderId}", notification.CustomerId, notification.OrderId);

            await repository.DeleteBasketAsync(notification.CustomerId);
        }
        catch (Exception ex)
        {
            // 🛑 CRITICAL: We CATCH the exception here.
            // We do NOT want to throw an error back to the Ordering module.
            // The Order is already saved. The user is happy. 
            // We just log this as a "Zombie Basket" to be fixed later.
            logger.LogError(ex, "Failed to clear basket for customer {CustomerId}. Basket is now a Zombie.", notification.CustomerId);
        }
    }
}