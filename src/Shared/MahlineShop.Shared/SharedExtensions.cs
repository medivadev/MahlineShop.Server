using MahlineShop.Shared.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MahlineShop.Shared;

public static class SharedExtensions
{
    public static IServiceCollection AddSharedFramework(this IServiceCollection services, Assembly[] moduleAssemblies)
    {
        // Add MediatR for the provided assemblies
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(moduleAssemblies);

            // Register the Validation Behavior automatically
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
