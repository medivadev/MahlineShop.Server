using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MahlineShop.Shared.Result;

namespace MahlineShop.Modules.Identity.Users.Features.RegisterUser;

public static class RegisterUserEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/register", async (RegisterUserCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            // 1. Check failure based on validation or handler logic
            if (result.IsFailure)
            {
                // Return 400 Bad Request if the handler returned a domain error
                return Results.BadRequest(result.Error);
            }

            // 2. Success - returns a clean 201 Created status
            return Results.StatusCode(StatusCodes.Status201Created);
        })
        .WithName("RegisterUser")
        .WithSummary("Registers a new user in the system.")
        .Produces(StatusCodes.Status201Created)
        .Produces<Error>(StatusCodes.Status400BadRequest);
    }
}