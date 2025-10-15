using VaultEdge.Application.Interfaces;
using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
            => await _accountRepository.GetAllAsync();

        public async Task<Account?> GetAccountByIdAsync(Guid id)
            => await _accountRepository.GetByIdAsync(id);

        public async Task<Account> DeleteAccountAsync(Guid id)
            => await _accountRepository.DeleteAccountAsync(id);

        public async Task<Account> CreateAccountAsync(Account account)
        {
            await _accountRepository.AddAsync(account);
            await _accountRepository.SaveChangesAsync();
            return account;
        }

        public async Task<Account?> DepositAsync(Guid accountId, decimal amount)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
                        if (account == null || amount <= 0)
                return null;
            account.Balance += amount;
            await _accountRepository.SaveChangesAsync();
            return account;
        }

        public async Task<Account?> WithdrawAsync(Guid accountId, decimal amount)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
                        if (account == null || amount <= 0 || account.Balance < amount)
                return null;
            account.Balance -= amount;
            await _accountRepository.SaveChangesAsync();
            return account;

        }
    }
}
