using ErrorOr;
using MapsterMapper;
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
        private readonly IMapper _mapper;

        public AuthenticationController( IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;

        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<SignupCommand>(request);

            ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);

            return authResult.Match(
                authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
                errors => Problem(errors)
            );
        }

        [HttpPost("signin")]
        public async Task<IActionResult> Signin(SigninRequest request, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<SigninQuery>(request);

            ErrorOr<AuthenticationResult> authResult = await _mediator.Send(query);

            if(authResult.IsError && authResult.FirstError == AuthenticationErrors.Authentication.InvalidCredentials)
            {
                return Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: authResult.FirstError.Description
                );
            }

            return authResult.Match(
                authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
                errors => Problem(errors)
            );
        }
    }
}