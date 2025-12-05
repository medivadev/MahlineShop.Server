using MahlineShop.API.Services;
using MahlineShop.Modules.Basket;
using MahlineShop.Modules.Catalog;
using MahlineShop.Modules.Identity;
using MahlineShop.Modules.Ordering;
using MahlineShop.Shared;
using MahlineShop.Shared.Services;
using Scalar.AspNetCore;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build())
    .CreateLogger();

try
{

    builder.Host.UseSerilog();

    // -----------------------------------------------------------------------------
    // 1. REGISTER SERVICES (DI)
    // -----------------------------------------------------------------------------

    // Get the assemblies for all our modules. 
    // Right now, we only have the API assembly. Later we will add Catalog, Basket, etc.
    Assembly[] moduleAssemblies = [ 
        typeof(Program).Assembly, 
        typeof(CatalogModuleExtensions).Assembly,
        typeof(IdentityModuleExtensions).Assembly,
        typeof(BasketModuleExtensions).Assembly,
        typeof(OrderingModuleExtensions).Assembly
    ];

    // Register our Shared Framework (MediatR, Behaviors, Validators)
    builder.Services.AddSharedFramework(moduleAssemblies);

    // Register the Catalog Module's services (DbContext, Configuration, etc.)
    builder.Services.AddCatalogModule(builder.Configuration);
    builder.Services.AddIdentityModule(builder.Configuration);
    builder.Services.AddBasketModule(builder.Configuration);
    builder.Services.AddOrderingModule(builder.Configuration);

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<ICurrentUser, CurrentUser>();

    builder.Services.AddAuthorization();

    // Add OpenAPI (Swagger)
    builder.Services.AddOpenApi();

    var app = builder.Build();

    // -----------------------------------------------------------------------------
    // 2. PIPELINE (Middleware)
    // -----------------------------------------------------------------------------

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi(); // .NET 10 built-in OpenAPI mapping
                          // You can add SwaggerUI here if you installed the specific package, 
                          // but raw OpenAPI JSON is the default in modern templates.

        // 2. Serve the Scalar UI
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("MahlineShop API")
                .WithTheme(ScalarTheme.Solarized)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
    }

    app.UseHttpsRedirection();

    // -----------------------------------------------------------------------------
    // 3. Add Exception Handling
    // -----------------------------------------------------------------------------
    app.UseExceptionHandler();

    // -----------------------------------------------------------------------------
    // 4. AUTHENTICATION & AUTHORIZATION
    // -----------------------------------------------------------------------------
    app.UseAuthentication();
    app.UseAuthorization();

    // -----------------------------------------------------------------------------
    // 5. ENDPOINTS
    // -----------------------------------------------------------------------------

    // Just a test endpoint to verify the API is alive
    // app.MapGet("/", () => "MahlineShop API is running!");

    // Mapping all endpoints here
    app.MapCatalogEndpoints();
    app.MapIdentityEndpoints();
    app.MapBasketEndpoints();
    app.MapOrderingEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}