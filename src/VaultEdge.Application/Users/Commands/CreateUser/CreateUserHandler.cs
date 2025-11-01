using MediatR;
using VaultEdge.Domain.Entities;
using VaultEdge.Domain.Repositories;

namespace VaultEdge.Application.Users.Comands.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                throw new InvalidOperationException("A user with this email already exists.");

            var newUser = new User(
                request.FirstName,
                request.LastName,
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
