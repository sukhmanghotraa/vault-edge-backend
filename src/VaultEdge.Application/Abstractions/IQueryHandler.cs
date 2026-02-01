using ErrorOr;
using MediatR;

namespace VaultEdge.Application.Abstractions;
public interface IQueryHandler<TQuery, TResponse> 
    : IRequestHandler<TQuery, ErrorOr<TResponse>>
    where TQuery : IQuery<TResponse>
{
}