using VaultEdge.Application.Common.Interfaces.Authentication;
using VaultEdge.Application.Repositories;
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

        public AuthenticationResult Signup(string firstName, string lastName, string passwordHash, DateTime dateOfBirth, string taxId, string identificationId, string nationality, string email, string phoneNumber, string address)
        {
            // check if user already exits
            var existingUserTask = _userRepository.GetByEmailAsync(email, default);
            var existingUser = existingUserTask.GetAwaiter().GetResult();

            if(existingUser is not null)
            {
                throw new Exception("User with given email already exists");
            }

            // Create user (generate unique ID)

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

            // Create JWT token

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token);
        }

        public AuthenticationResult Signin(string email, string password)
        {
            var existingUser = _userRepository.GetByEmailAsync(email, default).Result;

            if(existingUser is not User user)
            {
                throw new Exception("User with given email does not exist");
            }

            if(user.PasswordHash != password)
            {
                throw new Exception("Invalid password");
            }

            var token  = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token);
        }
    }
}
