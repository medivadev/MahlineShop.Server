using MahlineShop.Shared.CQRS;
using MahlineShop.Shared.Result;

namespace MahlineShop.Modules.Ordering.Orders.Features.CheckoutOrder;

// We allow the user to specify a separate shipping address here in the future
public record CheckoutOrderCommand(string ShippingAddress) : ICommand<Result<Guid>>;