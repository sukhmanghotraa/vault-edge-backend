using ErrorOr;
using MediatR;
using VaultEdge.Application.Authentication.Common;
using VaultEdge.Application.Common.Interfaces.Authentication;
using VaultEdge.Application.Common.Interfaces.Persistence;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Common.Errors;

namespace VaultEdge.Application.Authentication.Queries.Signin
{
    public class SigninQueryHandler: IRequestHandler<SigninQuery, ErrorOr<AuthenticationResult>>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ICustomerRepository _customerRepository;

        public SigninQueryHandler(IIdentityService identityService,IJwtTokenGenerator jwtTokenGenerator, ICustomerRepository customerRepository)
        {
            _identityService = identityService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _customerRepository = customerRepository;
        }


        public async Task<ErrorOr<AuthenticationResult>> Handle(SigninQuery query, CancellationToken cancellationToken)
        {
            var authResult = _identityService.AuthenticateAsync(query.Email, query.Password).Result;

            if (!authResult.Succeeded)
            {
                return AuthenticationErrors.Authentication.InvalidCredentials;
            }

            var customer = await _customerRepository.GetByIdAsync(authResult.CustomerId);

            if (customer is null)
            {
                return CustomerErrors.Customer.NotFound(authResult.CustomerId);
            }

            var accessToken  = _jwtTokenGenerator.GenerateAccesssToken(customer, authResult.SecurityStamp);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            return new AuthenticationResult(
                customer,
                accessToken,
                refreshToken);
        }
    }
}