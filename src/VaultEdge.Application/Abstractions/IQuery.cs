using ErrorOr;
using MediatR;

namespace VaultEdge.Application.Abstractions;

public interface IQuery<TResponse>: IRequest<ErrorOr<TResponse>>
{
}