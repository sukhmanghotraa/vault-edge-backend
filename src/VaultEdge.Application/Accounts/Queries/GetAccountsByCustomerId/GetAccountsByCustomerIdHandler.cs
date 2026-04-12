using ErrorOr;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Accounts.DTOs;
using VaultEdge.Application.Accounts.Queries.GetAllAccounts;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Common.Errors;

namespace VaultEdge.Application.Accounts.Queries.GetAccountsByCustomerId
{
    public class GetAccountsByCustomerIdHandler : IQueryHandler<GetAccountsByCustomerIdQuery, List<AccountDto>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountsByCustomerIdHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<ErrorOr<List<AccountDto>>> Handle(GetAccountsByCustomerIdQuery request, CancellationToken cancellationToken = default)
        {
            var accounts = await _accountRepository.GetByCustomerIdAsync(request.CustomerId);

            if (accounts is null)
            {
                return AccountErrors.Account.NoneFound;
            }

            var accountsCopy = accounts.Select(account => new AccountDto
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                CustomerId = account.CustomerId,
                AccountType = account.Type,
                Transactions = account.Transactions,
                CreatedAt = account.CreatedAt
            });

            return accountsCopy.ToList();
        }
    }
}