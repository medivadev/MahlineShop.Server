namespace MahlineShop.Modules.Ordering.Domain;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; } // Reference only
    public string ProductName { get; private set; } = string.Empty; // Snapshot
    public decimal Price { get; private set; } // Snapshot (Price at moment of purchase)
    public int Quantity { get; private set; }

    private OrderItem() { }

    internal OrderItem(Guid orderId, Guid productId, string productName, decimal price, int quantity)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
    }
}