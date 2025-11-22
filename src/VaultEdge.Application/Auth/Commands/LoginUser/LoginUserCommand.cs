using VaultEdge.Application.Abstractions;

namespace VaultEdge.Application.Auth.Commands.LoginUser
{
    public record LoginUserCommand(string Email) : ICommand<string>;
}