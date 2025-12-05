using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MahlineShop.Shared.Result;

namespace MahlineShop.Modules.Basket.Features.AddItemToBasket;

public static class AddItemToBasketEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/items", async (AddItemToBasketCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            // Return 200 OK with the new total price
            return Results.Ok(new { TotalPrice = result.Value });
        })
        .WithName("AddItemToBasket")
        .WithSummary("Adds an item to the user's shopping basket.")
        .Produces(StatusCodes.Status200OK)
        .Produces<Error>(StatusCodes.Status400BadRequest);
    }
}