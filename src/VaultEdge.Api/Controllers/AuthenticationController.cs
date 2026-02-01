using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using VaultEdge.Api.Authentication;
using VaultEdge.Application.Authentication;
using VaultEdge.Domain.Common.Errors;

namespace VaultEdge.Api.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;


        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("signup")]
        public IActionResult Signup(SignupRequest request, CancellationToken cancellationToken)
        {
            ErrorOr<AuthenticationResult> authResult = _authenticationService.Signup(
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

            return authResult.Match(
                authResult => Ok(CreateAuthenticationResponse(authResult)),
                errors => Problem(errors)
            );
        }

        [HttpPost("signin")]
        public IActionResult Signin(SigninRequest request, CancellationToken cancellationToken)
        {
            ErrorOr<AuthenticationResult> authResult = _authenticationService.Signin(
                request.Email, 
                request.Password);

            if(authResult.IsError && authResult.FirstError == AuthenticationErrors.Authentication.InvalidCredentials)
            {
                return Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: authResult.FirstError.Description
                );
            }

            return authResult.Match(
                authResult => Ok(CreateAuthenticationResponse(authResult)),
                errors => Problem(errors)
            );
        }

        private static AuthenticationResponse CreateAuthenticationResponse(AuthenticationResult authResult)
        {
            return new AuthenticationResponse(
                authResult.User.Id,
                authResult.User.FirstName,
                authResult.User.LastName,
                authResult.User.Email,
                authResult.Token);
        }
    }
}