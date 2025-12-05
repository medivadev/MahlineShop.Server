using MediatR;

namespace MahlineShop.Shared.Integration;

public record OrderCreatedEvent(Guid OrderId, string CustomerId) : INotification;
