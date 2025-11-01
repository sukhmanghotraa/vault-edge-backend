using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaultEdge.Application.Users.Comands.CreateUser;

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

        [HttpGet("{accountId}")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var userId = await _mediator.Send(command);
            return Ok(new { Id = userId });
        }
    }
}
