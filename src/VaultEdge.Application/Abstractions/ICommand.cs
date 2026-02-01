using ErrorOr;
using MediatR;

namespace VaultEdge.Application.Abstractions;
public interface ICommand: IRequest<ErrorOr<Unit>>
{
}

public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>
{
}