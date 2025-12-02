using MediatR;
using VaultEdge.Application.Accounts.DTOs;
using VaultEdge.Domain.Errors;
using VaultEdge.Domain.Repositories;
using VaultEdge.Domain.Shared;

namespace VaultEdge.Application.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, Result<AccountDto?>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountByIdHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Result<AccountDto?>> Handle(GetAccountByIdQuery request, CancellationToken cancelationToken = default)
        {
            var account = await _accountRepository.GetByIdAsync(request.Id);

            if(account == null)
            {
                return Result.Failure<AccountDto?>(DomainErrors.Account.NotFound(request.Id));
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
