using MahlineShop.Modules.Ordering.Data;
using MahlineShop.Modules.Ordering.Orders.Features.CheckoutOrder;
using MahlineShop.Modules.Ordering.Orders.Features.GetOrders;
using MahlineShop.Shared.Behaviors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MahlineShop.Modules.Ordering;

public static class OrderingModuleExtensions
{
    public static IServiceCollection AddOrderingModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("OrderingConnection");

        services.AddDbContext<OrderingDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }

    public static IEndpointRouteBuilder MapOrderingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/ordering")
            .WithTags("Ordering")
            .AddEndpointFilter<TrafficLoggingFilter>()
            .RequireAuthorization(); // Critical

        // We will map endpoints here in the next step
        CheckoutOrderEndpoint.MapEndpoint(group);
        GetOrdersEndpoint.MapEndpoint(group);

        return app;
    }
}