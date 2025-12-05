using MahlineShop.Shared.CQRS;
using MahlineShop.Shared.Result;

namespace MahlineShop.Modules.Identity.Users.Features.LoginUser;

// Returns the AuthenticationResultDto on success
public record LoginUserCommand(
    string Email,
    string Password) : ICommand<Result<AuthenticationResultDto>>;