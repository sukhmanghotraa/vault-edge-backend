using VaultEdge.Application.Abstractions;
using VaultEdge.Domain.Entities;
using VaultEdge.Domain.Repositories;
using VaultEdge.Domain.Shared;

namespace VaultEdge.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountHandler : ICommandHandler<CreateAccountCommand, Guid>
    {
        private readonly IAccountRepository _accountRepository;

        public CreateAccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Result<Guid>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var newAccount = new Account(
                request.AccountNumber,
                request.UserId,
                request.Balance
            );
            await _accountRepository.AddAsync(newAccount);
            await _accountRepository.SaveChangesAsync();
            return Result.Success(newAccount.Id);
        }
    }
}
