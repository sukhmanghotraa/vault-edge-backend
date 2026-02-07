using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaultEdge.Api.Authentication;
using VaultEdge.Application.Authentication.Commands.Signup;
using VaultEdge.Application.Authentication.Common;
using VaultEdge.Application.Authentication.Queries.Signin;
using VaultEdge.Domain.Common.Errors;

namespace VaultEdge.Api.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController : ApiController
    {
        private readonly IMediator _mediator;


        public AuthenticationController( IMediator mediator)
        {
            _mediator = mediator;

        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupRequest request, CancellationToken cancellationToken)
        {
            var command = new SignupCommand(
                request.FirstName,
                request.LastName,
                request.PasswordHash,
                request.DateOfBirth,
                request.TaxId,
                request.IdentificationId,
                request.Nationality,
                request.Email,
                request.PhoneNumber,
                request.Address);

            ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);

            return authResult.Match(
                authResult => Ok(CreateAuthenticationResponse(authResult)),
                errors => Problem(errors)
            );
        }

        [HttpPost("signin")]
        public async Task<IActionResult> Signin(SigninRequest request, CancellationToken cancellationToken)
        {
            var query = new SigninQuery(
                request.Email,
                request.Password);

            ErrorOr<AuthenticationResult> authResult = await _mediator.Send(query);

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