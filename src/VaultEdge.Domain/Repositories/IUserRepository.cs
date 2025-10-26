
using VaultEdge.Domain.Entities;

namespace VaultEdge.Domain.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByEmailAsync(string email);
    }
}
