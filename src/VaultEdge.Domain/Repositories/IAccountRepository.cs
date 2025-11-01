using VaultEdge.Domain.Entities;

namespace VaultEdge.Domain.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(Guid id);
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account> CreateAccountAsync(Account account);
        Task AddAsync(Account account);
        Task SaveChangesAsync();
        Task<Account> DeleteAccountAsync(Guid id);
    }
}
