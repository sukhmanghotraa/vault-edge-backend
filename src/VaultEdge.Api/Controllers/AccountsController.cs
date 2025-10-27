using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaultEdge.Application.Accounts.Queries.GetAccountById;

namespace VaultEdge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccountById(Guid accountId)
        {
            var query = new GetAccountByIdQuery { AccountId = accountId };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
