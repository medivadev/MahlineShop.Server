using MahlineShop.Modules.Basket.Features.AddItemToBasket;
using MahlineShop.Modules.Basket.Features.GetBasket;
using MahlineShop.Modules.Basket.Persistence;
using MahlineShop.Shared.Behaviors;
using MahlineShop.Shared.Integration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MahlineShop.Modules.Basket;

public static class BasketModuleExtensions
{
    public static IServiceCollection AddBasketModule(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("RedisCache");

        // 1. Configure Distributed Caching to use Redis
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = "Basket:"; // Prefix key with "Basket:" for easy management
        });

        // 2. Register the Basket Repository (the gateway to Redis)
        services.AddScoped<IBasketRepository, RedisBasketRepository>();

        services.AddScoped<IBasketIntegrationService, Integration.BasketIntegrationService>();

        return services;
    }

    public static IEndpointRouteBuilder MapBasketEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/basket")
                       .WithTags("Basket")
                       .AddEndpointFilter<TrafficLoggingFilter>()
                       .RequireAuthorization(); // 🔒 Critical: This enforces JWT check

        AddItemToBasketEndpoint.MapEndpoint(group);
        GetBasketEndpoint.MapEndpoint(group);

        return app;
    }
}