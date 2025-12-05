using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MahlineShop.Modules.Ordering.Orders.Features.GetOrders;

public static class GetOrdersEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new GetOrdersQuery());
            return Results.Ok(result.Value);
        })
        .WithName("GetOrders")
        .WithSummary("Retrieves the order history for the current user.");
    }
}
