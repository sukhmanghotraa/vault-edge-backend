using MediatR;
using VaultEdge.Application.Accounts.DTOs;
using VaultEdge.Domain.Errors;
using VaultEdge.Domain.Repositories;
using VaultEdge.Domain.Shared;

namespace VaultEdge.Application.Accounts.Queries.GetAllAccounts
{
    public class GetAllAccountsHandler : IRequestHandler<GetAllAccountsQuery, Result<IEnumerable<AccountDto>>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAllAccountsHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Result<IEnumerable<AccountDto>>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken = default)
        {
            var accounts = await _accountRepository.GetAllAsync();
            if (accounts == null)
            {
                return Result.Failure<IEnumerable<AccountDto>>(
                    DomainErrors.Account.NoneFound);
            }

            var accountsCopy = accounts.Select(account => new AccountDto
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                Balance = account.Balance,
                UserId = account.UserId,
                AccountType = account.AccountType,
                CreatedAt = account.CreatedAt
            });

            return Result.Success<IEnumerable<AccountDto>>(accountsCopy);
        }
    }
}
