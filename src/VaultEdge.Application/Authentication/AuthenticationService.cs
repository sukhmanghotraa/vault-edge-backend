using ErrorOr;
using VaultEdge.Application.Common.Interfaces.Authentication;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Common.Errors;
using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Authentication
{
    public class AuthenticationService: IAuthenticationService
    {

        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public ErrorOr<AuthenticationResult> Signup(string firstName, string lastName, string passwordHash, DateTime dateOfBirth, string taxId, string identificationId, string nationality, string email, string phoneNumber, string address)
        {
            var existingUserTask = _userRepository.GetByEmailAsync(email, default);
            var existingUser = existingUserTask.GetAwaiter().GetResult();

            if(existingUser is not null)
            {
                return UserErrors.User.EmailAlreadyInUse;
            }

            var user = new User(
                 firstName,
                lastName,
                passwordHash,
                dateOfBirth,
                taxId,
                identificationId,
                nationality,
                email,
                phoneNumber,
                address
            );

            _userRepository.AddAsync(user);
            _userRepository.SaveChangesAsync();

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token);
        }

        public ErrorOr<AuthenticationResult> Signin(string email, string password)
        {
            var existingUser = _userRepository.GetByEmailAsync(email, default).Result;

            if(existingUser is not User user)
            {
                return AuthenticationErrors.Authentication.InvalidCredentials;
            }

            if(user.PasswordHash != password)
            {
                return new[] { AuthenticationErrors.Authentication.InvalidCredentials };
            }

            var token  = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token);
        }
    }
}