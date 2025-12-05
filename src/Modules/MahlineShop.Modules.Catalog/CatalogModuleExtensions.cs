using MahlineShop.Modules.Catalog.Data;
using MahlineShop.Modules.Catalog.Products.Features.CreateProduct;
using MahlineShop.Modules.Catalog.Products.Features.GetProductById;
using MahlineShop.Shared.Behaviors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MahlineShop.Modules.Catalog;

public static class CatalogModuleExtensions
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CatalogConnection");

        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        return services;
    }

    public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/catalog/products")
            .WithTags("Catalog")
            .AddEndpointFilter<TrafficLoggingFilter>();

        // Register each feature's endpoint
        CreateProductEndpoint.MapEndpoint(group);
        GetProductByIdEndpoint.MapEndpoint(group);

        return app;
    }
}
