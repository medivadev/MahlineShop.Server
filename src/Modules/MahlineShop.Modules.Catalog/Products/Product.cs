namespace MahlineShop.Modules.Catalog.Products;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }

    // EF Core requires a parameterless constructor
    private Product() { }

    // Factory method allows validation BEFORE object creation
    public static Product Create(string name, string? description, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.", nameof(name));

        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");

        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = price,
            StockQuantity = stock
        };
    }

    public void UpdatePrice(decimal newPrice)
    {
        Price = newPrice >= 0 
            ? newPrice 
            : throw new ArgumentOutOfRangeException(nameof(newPrice), "Price cannot be negative.");
    }
}
