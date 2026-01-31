using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Errors;
using VaultEdge.Domain.Shared;

namespace VaultEdge.Application.Users.Commands.DeleteUser
{
    public class DeleteUserHandler: ICommandHandler<DeleteUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<Guid>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if(user == null)
            {
                return Result.Failure<Guid>(
                    DomainErrors.User.NotFound(request.UserId));
            }

            await _userRepository.DeleteUserAsync(user.Id);
            await _userRepository.SaveChangesAsync();
            return Result.Success(user.Id);
        }
    }
}
