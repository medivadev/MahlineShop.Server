using MahlineShop.Shared.CQRS;
using MahlineShop.Shared.Result;

namespace MahlineShop.Modules.Catalog.Products.Features.GetProductById;

// The Request
public record GetProductByIdQuery(Guid Id) 
    : IQuery<Result<ProductDto>>;

// The Response DTO
public record ProductDto(
    Guid Id, 
    string Name, 
    string? Description, 
    decimal Price, 
    int Stock);
