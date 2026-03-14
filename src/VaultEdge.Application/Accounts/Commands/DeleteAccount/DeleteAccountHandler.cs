using ErrorOr;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Common.Errors;

namespace VaultEdge.Application.Accounts.Commands.DeleteAccount
{
    public class DeleteAccountHandler : ICommandHandler<DeleteAccountCommand, Guid>
    {
        private readonly IAccountRepository _accountRepository;

        public DeleteAccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<ErrorOr<Guid>> Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(command.AccountId);

            if (account == null)
            {
                return AccountErrors.Account.NotFound(command.AccountId);
            }

            _accountRepository.Delete(account);
            await _accountRepository.SaveChangesAsync();
            return account.Id;
        }
    }
}
