using MahlineShop.Modules.Catalog.Data;
using MahlineShop.Shared.CQRS;

namespace MahlineShop.Modules.Catalog.Products.Features.CreateProduct;

internal class CreateProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // 1. Create Domain Entity
        var product = Product.Create(
            name: command.Name,
            description: command.Description,
            price: command.Price,
            stock: command.Stock);

        // 2. Persist
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        // 3. Return Result
        return new CreateProductResult(product.Id);
    }
}
