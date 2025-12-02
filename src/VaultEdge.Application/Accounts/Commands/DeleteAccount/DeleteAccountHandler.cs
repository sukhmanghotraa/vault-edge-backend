using VaultEdge.Application.Abstractions;
using VaultEdge.Domain.Errors;
using VaultEdge.Domain.Repositories;
using VaultEdge.Domain.Shared;

namespace VaultEdge.Application.Accounts.Commands.DeleteAccount
{
    public class DeleteAccountHandler : ICommandHandler<DeleteAccountCommand, Guid>
    {
        private readonly IAccountRepository _accountRepository;

        public DeleteAccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Result<Guid>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(request.AccountId);
            if (account == null)
            {
                return Result.Failure<Guid>(
                    DomainErrors.Account.NotFound(request.AccountId));
            }

            await _accountRepository.DeleteAccountAsync(account.Id);
            await _accountRepository.SaveChangesAsync();
            return Result.Success(account.Id);
        }
    }
}
