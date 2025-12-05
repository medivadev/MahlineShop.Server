using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MahlineShop.Modules.Catalog.Products.Features.CreateProduct;

public static class CreateProductEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CreateProductCommand request, ISender sender) =>
        {
            var result = await sender.Send(request);

            return Results.Created($"/api/catalog/products/{result.Id}", result);
        })
        .WithName("CreateProduct")
        .WithSummary("Creates a new product in the catalog")
        .WithDescription("Requires a unique name and positive price.");

    }
}
