using MahlineShop.Shared.CQRS;

namespace MahlineShop.Modules.Catalog.Products.Features.CreateProduct;

// The Request Object
public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock) 
    : ICommand<CreateProductResult>;

// The Response Object
public record CreateProductResult(Guid Id);


