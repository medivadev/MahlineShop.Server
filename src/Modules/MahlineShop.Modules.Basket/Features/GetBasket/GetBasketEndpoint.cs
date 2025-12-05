using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MahlineShop.Modules.Basket.Features.GetBasket;

public static class GetBasketEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new GetBasketQuery());

            return Results.Ok(result.Value);
        })
        .WithName("GetBasket")
        .WithSummary("Retrieves the current user's basket.")
        .Produces<MahlineShop.Modules.Basket.Domain.Basket>(StatusCodes.Status200OK);
    }
}