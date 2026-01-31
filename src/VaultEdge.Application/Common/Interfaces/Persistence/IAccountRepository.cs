using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Repositories
{
    public interface IAccountRepository
    {
        Task AddAsync(Account account);
        Task<Account?> GetByIdAsync(Guid id);
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account> CreateAccountAsync(Account account);
        Task SaveChangesAsync();
        Task<Guid> DeleteAccountAsync(Guid id);
    }
}
