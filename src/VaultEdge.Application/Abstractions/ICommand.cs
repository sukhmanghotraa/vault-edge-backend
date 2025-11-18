using MediatR;
using VaultEdge.Domain.Shared;

namespace VaultEdge.Application.Abstractions;
public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
