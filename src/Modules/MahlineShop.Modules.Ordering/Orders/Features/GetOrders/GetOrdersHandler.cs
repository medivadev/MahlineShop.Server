using MahlineShop.Modules.Ordering.Data;
using MahlineShop.Shared.CQRS;
using MahlineShop.Shared.Result;
using MahlineShop.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace MahlineShop.Modules.Ordering.Orders.Features.GetOrders;

internal class GetOrdersHandler(
    OrderingDbContext dbContext,
    ICurrentUser currentUser)
    : IQueryHandler<GetOrdersQuery, Result<List<OrderDto>>>
{
    public async Task<Result<List<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;

        var orders = await dbContext.Orders
            .Where(o => o.CustomerId == userId)
            .AsNoTracking()
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new OrderDto(o.Id, o.OrderNumber, o.CreatedAt, o.TotalAmount, o.Status.ToString()))
            .ToListAsync(cancellationToken);

        return Result<List<OrderDto>>.Success(orders);
    }
}
