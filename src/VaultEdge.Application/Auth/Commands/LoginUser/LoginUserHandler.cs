using VaultEdge.Application.Abstractions;
using VaultEdge.Domain.Entities;
using VaultEdge.Domain.Errors;
using VaultEdge.Domain.Repositories;
using VaultEdge.Domain.Shared;

namespace VaultEdge.Application.Auth.Commands.LoginUser
{
    internal sealed class LoginUserHandler : ICommandHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public LoginUserHandler(IUserRepository userRepository, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // get user by customer id
            User? user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user == null)
            {
                return Result.Failure<string>(
                    DomainErrors.User.InvalidCredentials);
            }

            // Generate JWT
            string token = _jwtProvider.Generate(user);

            // Return JWT

            return token;
        }
    }
}