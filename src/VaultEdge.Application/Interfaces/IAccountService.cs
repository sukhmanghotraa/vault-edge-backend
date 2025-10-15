using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Interfaces
{
    public interface IAccountService
    {
        Task<Account?> GetAccountByIdAsync(Guid id);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account> CreateAccountAsync(Account account);
        Task<Account> DeleteAccountAsync(Guid id);
        Task<Account?> DepositAsync(Guid accountId, decimal amount);
        Task<Account?> WithdrawAsync(Guid accountId, decimal amount);
    }
}
