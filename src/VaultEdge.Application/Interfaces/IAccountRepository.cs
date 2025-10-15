using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(Guid id);
        Task<IEnumerable<Account>> GetAllAsync();
        Task AddAsync(Account account);
        Task SaveChangesAsync();
        Task<Account> DeleteAccountAsync(Guid id);
    }
}
