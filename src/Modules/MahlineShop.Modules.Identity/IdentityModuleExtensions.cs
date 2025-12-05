using MahlineShop.Modules.Identity.Services;
using MahlineShop.Modules.Identity.Users.Features.LoginUser;
using MahlineShop.Modules.Identity.Users.Features.RegisterUser;
using MahlineShop.Shared.Behaviors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MahlineShop.Modules.Identity;

public static class IdentityModuleExtensions
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Add IdentityDbContext
        services.AddDbContext<Data.IdentityDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
        });
        // Add Identity services
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<Data.IdentityDbContext>()
        .AddDefaultTokenProviders();


        // --- JWT & Authentication Configuration ---

        // 2. Bind JWT Settings
        var jwtSettings = configuration.GetSection(JwtSettings.SettingsKey).Get<JwtSettings>()
            ?? throw new InvalidOperationException("JwtSettings not found in configuration.");
        services.AddSingleton(jwtSettings);

        // 3. Register the JWT Token Generator Service
        services.AddScoped<IJwtService, JwtService>();

        // 4. Configure Authentication Scheme (Required for token validation later)
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            };
        });


        return services;
    }

    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/identity/users")
                       .WithTags("Identity")
                       .AddEndpointFilter<TrafficLoggingFilter>();

        // ⬇️ NEW: Map the registration endpoint
        RegisterUserEndpoint.MapEndpoint(group);
        LoginUserEndpoint.MapEndpoint(group);

        return app;
    }
}
