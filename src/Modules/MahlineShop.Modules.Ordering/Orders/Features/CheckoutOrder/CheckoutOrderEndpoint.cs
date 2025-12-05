using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MahlineShop.Shared.Result;

namespace MahlineShop.Modules.Ordering.Orders.Features.CheckoutOrder;

public static class CheckoutOrderEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/checkout", async (CheckoutOrderCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok(new { OrderId = result.Value });
        })
        .WithName("CheckoutOrder")
        .WithSummary("Converts the user's basket into a permanent order.")
        .Produces(StatusCodes.Status200OK)
        .Produces<Error>(StatusCodes.Status400BadRequest);
    }
}