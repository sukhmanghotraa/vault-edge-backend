using MediatR;
using VaultEdge.Domain.Shared;

namespace VaultEdge.Application.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}