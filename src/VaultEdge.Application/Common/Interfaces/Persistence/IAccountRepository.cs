using VaultEdge.Domain.Account;

namespace VaultEdge.Application.Repositories
{
    public interface IAccountRepository
    {
        Task AddAsync(Account account, CancellationToken cancellationToken = default);
        Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
        Task<List<Account>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task<List<Account>> GetAllAsync(CancellationToken cancellationToken = default);
        void Update(Account account);
        void Delete(Account account);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
