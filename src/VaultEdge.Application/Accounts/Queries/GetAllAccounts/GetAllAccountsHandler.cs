using ErrorOr;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Accounts.DTOs;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Common.Errors;

namespace VaultEdge.Application.Accounts.Queries.GetAllAccounts
{
    public class GetAllAccountsHandler : IQueryHandler<GetAllAccountsQuery, List<AccountDto>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAllAccountsHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<ErrorOr<List<AccountDto>>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken = default)
        {
            var accounts = await _accountRepository.GetAllAsync();
            if (accounts == null)
            {
                return AccountErrors.Account.NoneFound;
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

            return accountsCopy.ToList();
        }
    }
}