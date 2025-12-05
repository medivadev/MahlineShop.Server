using MahlineShop.Modules.Catalog.Data;
using MahlineShop.Shared.CQRS;
using MahlineShop.Shared.Result;
using Microsoft.EntityFrameworkCore;

namespace MahlineShop.Modules.Catalog.Products.Features.GetProductById;

internal class GetProductByIdHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .Where(p => p.Id == query.Id)
            .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.StockQuantity))
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            return Result<ProductDto>.Failure(new Error("Product.NotFound", $"Product with Id {query.Id} was not found."));
        }

        return Result<ProductDto>.Success(product);
    }
}
