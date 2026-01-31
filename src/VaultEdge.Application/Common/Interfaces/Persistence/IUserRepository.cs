
using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User?> GetByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> CreateUserAsync(User user);
        Task SaveChangesAsync();
        Task<Guid> DeleteUserAsync(Guid id);
    }
}
