using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaultEdge.Application.Accounts.Commands.CreateAccount;
using VaultEdge.Application.Accounts.Commands.DeleteAccount;
using VaultEdge.Application.Accounts.Queries.GetAccountById;
using VaultEdge.Application.Accounts.Queries.GetAllAccounts;

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

            if(!accountId.IsSuccess)
            {
                return BadRequest(accountId.Error);
            }

            return Ok(new { AccountId = accountId });
        }

        [Authorize]
        [HttpGet("{accountId:guid}")]
        public async Task<IActionResult> GetAccountById(Guid accountId)
        {
            var query = new GetAccountByIdQuery(accountId);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{accountId:guid}")]
        public async Task<IActionResult> DeleteAccountById(Guid accountId)
        {
            var query = new DeleteAccountCommand(accountId);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var query = new GetAllAccountsQuery();
            var accounts = await _mediator.Send(query);

            if (!accounts.IsSuccess)
            {
                return BadRequest(accounts.Error);
            }

            return Ok(accounts);
        }
    }
}
