using Microsoft.AspNetCore.Mvc;
using VaultEdge.Api.Authentication;
using VaultEdge.Application.Authentication;

namespace VaultEdge.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;


        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("signup")]
        public IActionResult Signup(SignupRequest request, CancellationToken cancellationToken)
        {
            var authResult = _authenticationService.Signup(
                request.FirstName, 
                request.LastName, 
                request.Email, 
                request.Password);

            var response = new AuthenticationResponse(
                authResult.Id,
                authResult.FirstName,
                authResult.LastName,
                authResult.Email,
                authResult.Token);

            return Ok(authResult);
        }

        [HttpPost("signin")]
        public IActionResult Signin(SigninRequest request, CancellationToken cancellationToken)
        {
            var authResult = _authenticationService.Signin(
                request.Email, 
                request.Password);

            var response = new AuthenticationResponse(
                authResult.Id,
                authResult.FirstName,
                authResult.LastName,
                authResult.Email,
                authResult.Token);

            return Ok(authResult);
        }
    }
}
