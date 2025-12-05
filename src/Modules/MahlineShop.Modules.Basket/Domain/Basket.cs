namespace MahlineShop.Modules.Basket.Domain;

public record Basket
{
    public string CustomerId { get; init; } = string.Empty;
    public List<BasketItem> Items { get; init; } = [];
    public decimal TotalPrice => Items.Sum(i => i.Quantity * i.UnitPrice);
}

public record BasketItem
{
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
}