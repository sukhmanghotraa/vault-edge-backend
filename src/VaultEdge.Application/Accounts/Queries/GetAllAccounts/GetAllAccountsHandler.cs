using MediatR;
using VaultEdge.Application.Accounts.DTOs;
using VaultEdge.Domain.Repositories;

namespace VaultEdge.Application.Accounts.Queries.GetAllAccounts
{
    public class GetAllAccountsHandler : IRequestHandler<GetAllAccountsQuery, IEnumerable<AccountDto>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAllAccountsHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<AccountDto>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken = default)
        {
            var accounts = await _accountRepository.GetAllAsync();
            if (accounts == null)
            {
                return Enumerable.Empty<AccountDto>();
            }

            return accounts.Select(account => new AccountDto
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                Balance = account.Balance,
                UserId = account.UserId,
                AccountType = account.AccountType,
                CreatedAt = account.CreatedAt
            });
        }
    }
}
