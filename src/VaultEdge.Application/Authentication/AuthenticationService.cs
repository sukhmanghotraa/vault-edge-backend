using VaultEdge.Application.Common.Interfaces.Authentication;

namespace VaultEdge.Application.Authentication
{
    public class AuthenticationService: IAuthenticationService
    {

        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public AuthenticationResult Signup(string firstName, string lastName, string email, string password)
        {
            // check if user already exits

            // Create user (generate unique ID)

            // Create JWT token
            Guid userId = Guid.NewGuid();

            var token = _jwtTokenGenerator.GenerateToken(userId, firstName, lastName, email);

            return new AuthenticationResult(
                userId,
                firstName, 
                lastName, 
                email, 
                token);
        }

        public AuthenticationResult Signin(string email, string password)
        {
            return new AuthenticationResult(
                Guid.NewGuid(),
                "firstName",
                "lastName",
                email,
                "token");
        }
    }
}
