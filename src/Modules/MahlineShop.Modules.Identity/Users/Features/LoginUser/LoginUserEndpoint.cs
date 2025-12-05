using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MahlineShop.Shared.Result;

namespace MahlineShop.Modules.Identity.Users.Features.LoginUser;

public static class LoginUserEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async (LoginUserCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                // Returns 400 Bad Request with the InvalidCredentialsError
                return Results.BadRequest(result.Error);
            }

            // Success: returns 200 OK with the Token DTO
            return Results.Ok(result.Value);
        })
        .WithName("LoginUser")
        .WithSummary("Logs in a user and returns a JWT token.")
        .Produces<AuthenticationResultDto>(StatusCodes.Status200OK)
        .Produces<Error>(StatusCodes.Status400BadRequest);
    }
}