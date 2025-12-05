namespace MahlineShop.Modules.Ordering.Domain;

public class Order
{
    public Guid Id { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty; // Human readable: "ORD-2025-123"
    public string CustomerId { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }

    // Navigation Property
    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { } // For EF Core

    public static Order Create(string customerId)
    {
        return new Order
        {
            Id = Guid.NewGuid(),
            OrderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{customerId[..4].ToUpper()}",
            CustomerId = customerId,
            CreatedAt = DateTime.Now,
            Status = OrderStatus.Pending
        };
    }

    public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        var item = new OrderItem(Id, productId, productName, unitPrice, quantity);
        _items.Add(item);

        // Recalculate total immediately
        TotalAmount += item.Price * item.Quantity;
    }
}

public enum OrderStatus
{
    Pending = 1,
    PaymentFailed = 2,
    Paid = 3,
    Shipped = 4,
    Cancelled = 5
}