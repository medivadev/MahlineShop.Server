using MahlineShop.Shared.CQRS;
using MahlineShop.Shared.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MahlineShop.Modules.Identity.Users.Features.RegisterUser;

internal class RegisterUserHandler(UserManager<IdentityUser> userManager)
    : ICommandHandler<RegisterUserCommand, Result>
{
    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Check if user already exists
        var existingUser = await userManager.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (existingUser is not null)
        {
            return Result.Failure(new Error("User.AlreadyExists", "A user with this email already exists."));
        }

        // 2. Create the Identity User object
        var user = new IdentityUser
        {
            UserName = request.Email, // Use email as username
            Email = request.Email
        };

        // 3. Attempt to create the user
        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            // 4. Map Identity errors to our custom Shared.Result.Error structure
            var errors = result.Errors.Select(e => new Error(e.Code, e.Description)).ToList();

            // Return the first error or a generic failure
            return Result.Failure(errors.FirstOrDefault() ?? Error.InternalServerError);
        }

        return Result.Success();
    }
}