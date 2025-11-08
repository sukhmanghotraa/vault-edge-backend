using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaultEdge.Application.Accounts.Queries.GetAccountById;
using VaultEdge.Application.Accounts.Queries.GetAllAccounts;
using VaultEdge.Application.Accounts.Commands.CreateAccount;

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

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command)
        {
            var accountId = await _mediator.Send(command);
            return Ok(new { AccountId = accountId });
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccountById(Guid accountId)
        {
            var query = new GetAccountByIdQuery(accountId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var query = new GetAllAccountsQuery();
            var accounts = await _mediator.Send(query);
            return Ok(accounts);
        }
    }
}
