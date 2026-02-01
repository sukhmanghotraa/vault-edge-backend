using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaultEdge.Application.Accounts.Commands.CreateAccount;
using VaultEdge.Application.Accounts.Commands.DeleteAccount;
using VaultEdge.Application.Accounts.Queries.GetAccountById;
using VaultEdge.Application.Accounts.Queries.GetAllAccounts;

namespace VaultEdge.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : ApiController
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                accountId => CreatedAtAction(nameof(GetAccountById), new { accountId }, accountId),
                _ => Problem(statusCode: StatusCodes.Status409Conflict, title: "Account already exit.")
            );
        }

        [HttpGet("{accountId:guid}")]
        public async Task<IActionResult> GetAccountById(Guid accountId)
        {
            var query = new GetAccountByIdQuery(accountId);
            var result = await _mediator.Send(query);

            return result.Match(
                account => Ok(account),
                _ => Problem(statusCode: StatusCodes.Status404NotFound, title: "Account not found.")
            );
        }

        [HttpDelete("{accountId:guid}")]
        public async Task<IActionResult> DeleteAccountById(Guid accountId)
        {
            var query = new DeleteAccountCommand(accountId);
            var result = await _mediator.Send(query);

            return result.Match(
                deletedAccountId => Ok(deletedAccountId),
                _ => Problem(statusCode: StatusCodes.Status404NotFound, title: "Account not found.")
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var query = new GetAllAccountsQuery();
            var accounts = await _mediator.Send(query);

            return accounts.Match(
                accountList => Ok(accountList),
                _ => Problem(statusCode: StatusCodes.Status404NotFound, title: "No accounts found.")
            );
        }
    }
}