using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaultEdge.Application.Auth.Commands.LoginUser;
using VaultEdge.Application.Users.Commands.CreateUser;
using VaultEdge.Application.Users.Commands.DeleteUser;
using VaultEdge.Application.Users.Queries.GetAllUsers;
using VaultEdge.Application.Users.Queries.GetUserById;
using VaultEdge.Domain.Shared;

namespace VaultEdge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var userId = await _mediator.Send(command); 

            if (!userId.IsSuccess)
            {
                return BadRequest(userId.Error);
            }

            return Ok(new { UserId = userId });
        }

        [Authorize]
        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var authenticatedUserId = User.FindFirst("sub")?.Value;

            // This only exists if authorization succeeded
            Console.WriteLine($"Authorized user: {authenticatedUserId}");

            var query = new GetUserByIdQuery(userId);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }

        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> DeleteUserById(Guid userId)
        {
            var query = new DeleteUserCommand(userId);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            var users = await _mediator.Send(query);

            if (!users.IsSuccess)
            {
                return BadRequest(users.Error);
            }
            return Ok(users);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginUserCommand(request.Email);

            Result<string> tokenResult = await _mediator.Send(command, cancellationToken);

            if (tokenResult == null)
            {
                return Unauthorized(tokenResult);
            }

            return Ok(tokenResult.Value);
        }
    }
}
