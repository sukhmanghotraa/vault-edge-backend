using ErrorOr;
using MediatR;
using VaultEdge.Application.Authentication.Common;
using VaultEdge.Application.Common.Interfaces.Authentication;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Common.Errors;
using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Authentication.Commands.Signup
{
    public class SignupCommandHandler: IRequestHandler<SignupCommand, ErrorOr<AuthenticationResult>>
    {

        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public SignupCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(SignupCommand command, CancellationToken cancellationToken)
        {
            var existingUserTask = _userRepository.GetByEmailAsync(command.Email, default);
            var existingUser = existingUserTask.GetAwaiter().GetResult();

            if(existingUser is not null)
            {
                return UserErrors.User.EmailAlreadyInUse;
            }

            var user = new User(
                command.FirstName,
                command.LastName,
                command.PasswordHash,
                command.DateOfBirth,
                command.TaxId,
                command.IdentificationId,
                command.Nationality,
                command.Email,
                command.PhoneNumber,
                command.Address
            );

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token);
        }
    }
}