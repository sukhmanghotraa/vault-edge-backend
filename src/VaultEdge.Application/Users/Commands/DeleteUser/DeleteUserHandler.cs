using ErrorOr;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Common.Errors;

namespace VaultEdge.Application.Users.Commands.DeleteUser
{
    public class DeleteUserHandler: ICommandHandler<DeleteUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<Guid>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if(user == null)
            {
                return UserErrors.User.NotFound(request.UserId);
            }

            await _userRepository.DeleteUserAsync(user.Id);
            await _userRepository.SaveChangesAsync();
            return user.Id;
        }
    }
}