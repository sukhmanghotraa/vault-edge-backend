using ErrorOr;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Common.Errors;
using VaultEdge.Domain.User;

namespace VaultEdge.Application.Users.Commands.CreateUser
{
    public class CreateUserHandler : ICommandHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingUser != null)
            {
                return UserErrors.User.EmailAlreadyInUse;
            }

            var newUser = new User(
                request.FirstName,
                request.LastName,
                request.PasswordHash,
                request.DateOfBirth,
                request.TaxId,
                request.IdentificationId,
                request.Nationality,
                request.Email,
                request.PhoneNumber,
                request.Address
            );

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();
            return newUser.Id;
        }
    }
}
