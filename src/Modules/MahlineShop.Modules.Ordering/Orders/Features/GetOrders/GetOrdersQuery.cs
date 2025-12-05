using MahlineShop.Shared.CQRS;
using MahlineShop.Shared.Result;

namespace MahlineShop.Modules.Ordering.Orders.Features.GetOrders;

public record GetOrdersQuery() : IQuery<Result<List<OrderDto>>>;

// Simple DTO to avoid exposing the Domain Entity fully
public record OrderDto(Guid Id, string OrderNumber, DateTime CreatedAt, decimal TotalAmount, string Status);