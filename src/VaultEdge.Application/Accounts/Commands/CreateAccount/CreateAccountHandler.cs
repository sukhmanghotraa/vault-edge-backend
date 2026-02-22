using ErrorOr;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Account;

namespace VaultEdge.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountHandler : ICommandHandler<CreateAccountCommand, Guid>
    {
        private readonly IAccountRepository _accountRepository;

        public CreateAccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<ErrorOr<Guid>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var newAccount = new Account(
                request.AccountNumber,
                request.UserId,
                request.Balance
            );

            await _accountRepository.AddAsync(newAccount);
            await _accountRepository.SaveChangesAsync();
            return newAccount.Id;
        }
    }
}
