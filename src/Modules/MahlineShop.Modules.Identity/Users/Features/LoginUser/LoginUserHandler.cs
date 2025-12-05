using MahlineShop.Shared.CQRS;
using MahlineShop.Shared.Result;
using Microsoft.AspNetCore.Identity;
using MahlineShop.Modules.Identity.Services;

namespace MahlineShop.Modules.Identity.Users.Features.LoginUser;

internal class LoginUserHandler(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    IJwtService jwtService)
    : ICommandHandler<LoginUserCommand, Result<AuthenticationResultDto>>
{
    private static readonly Error InvalidCredentialsError =
        new("Auth.InvalidCredentials", "Invalid email or password.");

    public async Task<Result<AuthenticationResultDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Find User
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result<AuthenticationResultDto>.Failure(InvalidCredentialsError);
        }

        // 2. Check Password
        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            return Result<AuthenticationResultDto>.Failure(InvalidCredentialsError);
        }

        // 3. Get Roles (Future use for Authorization)
        var roles = await userManager.GetRolesAsync(user);

        // 4. Generate JWT Token
        var token = jwtService.CreateToken(user, roles);

        // 5. Return success result with the token DTO
        return Result<AuthenticationResultDto>.Success(new AuthenticationResultDto(token));
    }
}