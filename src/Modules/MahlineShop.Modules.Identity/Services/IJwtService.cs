using Microsoft.AspNetCore.Identity;

namespace MahlineShop.Modules.Identity.Services;

public interface IJwtService
{
    // The core method: creates a token for a validated user
    string CreateToken(IdentityUser user, IList<string> roles);
}
