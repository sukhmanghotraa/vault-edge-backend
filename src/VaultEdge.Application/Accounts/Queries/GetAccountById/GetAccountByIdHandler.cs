using MediatR;
using VaultEdge.Application.Accounts.DTOs;
using VaultEdge.Domain.Repositories;

namespace VaultEdge.Application.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, AccountDto?>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountByIdHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountDto?> Handle(GetAccountByIdQuery request, CancellationToken cancelationToken = default)
        {
            var account = await _accountRepository.GetByIdAsync(request.Id);
            if(account == null)
            {
                return null;
            }

            return new AccountDto
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                Balance = account.Balance,
                UserId = account.UserId,
                AccountType = account.AccountType,
                CreatedAt = account.CreatedAt,
            };
        }
    }
}
