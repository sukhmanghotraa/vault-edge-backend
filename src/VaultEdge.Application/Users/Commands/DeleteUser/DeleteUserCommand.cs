using VaultEdge.Application.Abstractions;

namespace VaultEdge.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : ICommand<Guid>
    {
        public Guid UserId { get; set; }

        public DeleteUserCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
