using Azure.Core;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaultEdge.Application.Accounts.Commands.CreateAccount;
using VaultEdge.Application.Accounts.Commands.DeleteAccount;
using VaultEdge.Application.Accounts.Queries.GetAccountById;
using VaultEdge.Application.Accounts.Queries.GetAccountsByCustomerId;
using VaultEdge.Application.Accounts.Queries.GetAllAccounts;
using VaultEdge.Application.Authentication.Commands.Signup;

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

        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command);

            ErrorOr<Guid> creationResult = await _mediator.Send(command);

            return result.Match(
                accountId => CreatedAtAction(nameof(GetAccountById), new { accountId }, accountId),
                errors => Problem(errors)
            );
        }

        [HttpGet("{accountId:guid}")]
        public async Task<IActionResult> GetAccountById(Guid accountId)
        {
            var query = new GetAccountByIdQuery(accountId);
            var result = await _mediator.Send(query);

            return result.Match(
                account => Ok(account),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{accountId:guid}")]
        public async Task<IActionResult> DeleteAccountById(Guid accountId)
        {
            var query = new DeleteAccountCommand(accountId);
            var result = await _mediator.Send(query);

            return result.Match(
                deletedAccountId => Ok(deletedAccountId),
                errors => Problem(errors)
            );
        }

        [HttpGet("all/{customerId:guid}")]
        public async Task<IActionResult> GetAccountsByCustomerId(Guid customerId)
        {
            var query = new GetAccountsByCustomerIdQuery(customerId);
            var accounts = await _mediator.Send(query);

            return accounts.Match(
                accountList => Ok(accountList),
                errors => Problem(errors)
            );
        }
    }
}