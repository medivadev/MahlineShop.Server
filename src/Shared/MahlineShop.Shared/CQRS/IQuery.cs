using MediatR;

namespace MahlineShop.Shared.CQRS;

// Queries ALWAYS return data. There is no "void" Query.
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
