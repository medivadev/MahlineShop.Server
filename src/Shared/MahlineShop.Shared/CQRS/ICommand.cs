using MediatR;

namespace MahlineShop.Shared.CQRS;

// A Command returns specific data (Result<T>)
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

// A Command that returns nothing (void)
public interface ICommand : IRequest<Unit>
{
}
