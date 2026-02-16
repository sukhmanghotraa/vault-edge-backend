using ErrorOr;
using MediatR;
using VaultEdge.Application.Authentication.Common;
using VaultEdge.Application.Common.Interfaces.Authentication;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Common.Errors;
using VaultEdge.Domain.User;

namespace VaultEdge.Application.Authentication.Queries.Signin
{
    public class SigninQueryHandler: IRequestHandler<SigninQuery, ErrorOr<AuthenticationResult>>
    {

        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public SigninQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }


        public async Task<ErrorOr<AuthenticationResult>> Handle(SigninQuery query, CancellationToken cancellationToken)
        {
            var existingUser = _userRepository.GetByEmailAsync(query.Email, default).Result;

            if(existingUser is not User user)
            {
                return AuthenticationErrors.Authentication.InvalidCredentials;
            }

            if(user.PasswordHash != query.Password)
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