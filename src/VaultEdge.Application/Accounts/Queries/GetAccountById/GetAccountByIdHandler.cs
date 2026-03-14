using ErrorOr;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Accounts.DTOs;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Common.Errors;

namespace VaultEdge.Application.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdHandler : IQueryHandler<GetAccountByIdQuery, AccountDto>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountByIdHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<ErrorOr<AccountDto>> Handle(GetAccountByIdQuery request, CancellationToken cancelationToken = default)
        {
            var account = await _accountRepository.GetByIdAsync(request.AccountId);

            if(account is null)
            {
                return AccountErrors.Account.NotFound(request.AccountId);
            }

            return new AccountDto
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                CustomerId = account.CustomerId,
                AccountType = account.Type,
                Transactions = account.Transactions,
                CreatedAt = account.CreatedAt,
            };
        }
    }
}