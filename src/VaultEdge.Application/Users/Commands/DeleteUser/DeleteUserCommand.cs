using VaultEdge.Application.Abstractions;

namespace VaultEdge.Application.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(Guid UserId) : ICommand<Guid>;
}
