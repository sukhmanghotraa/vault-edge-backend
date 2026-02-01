using ErrorOr;
using MediatR;

namespace VaultEdge.Application.Abstractions;

public interface ICommandHandler<TCommand>
    : IRequestHandler<TCommand, ErrorOr<Unit>>
    where TCommand : ICommand
{ 
}

public interface ICommandHandler<TCommand, TResponse> 
    : IRequestHandler<TCommand, ErrorOr<TResponse>>
    where TCommand : ICommand<TResponse>
{
}