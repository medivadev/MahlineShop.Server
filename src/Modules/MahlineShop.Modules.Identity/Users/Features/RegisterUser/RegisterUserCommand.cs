using MahlineShop.Shared.CQRS;
using MahlineShop.Shared.Result;

namespace MahlineShop.Modules.Identity.Users.Features.RegisterUser;

// The Command returns a simple success/failure result
public record RegisterUserCommand(
    string Email,
    string Password,
    string ConfirmPassword) : ICommand<Result>;
