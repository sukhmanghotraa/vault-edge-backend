using MediatR;
using VaultEdge.Domain.Shared;

namespace VaultEdge.Application.Abstractions;
public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}