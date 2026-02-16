using System.Collections.Concurrent;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.User;

namespace VaultEdge.Infrastructure.Persistence.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private static readonly ConcurrentDictionary<Guid, User> _users = new();

        public Task AddAsync(User user)
        {
            _users[user.Id] = user;
            return Task.CompletedTask;
        }

        public Task<User> CreateUserAsync(User user)
        {
            _users[user.Id] = user;
            return Task.FromResult(user);
        }

        public Task<User?> GetByIdAsync(Guid id)
        {
            _users.TryGetValue(id, out var user);
            return Task.FromResult(user);
        }

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = _users.Values.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }

        public Task<User?> GetByCustomerIdAsync(Guid customerId)
        {
            var user = _users.Values.FirstOrDefault(u => u.CustomerId == customerId);
            return Task.FromResult(user);
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            return Task.FromResult(_users.Values.AsEnumerable());
        }

        public Task SaveChangesAsync()
        {
            // No-op for in-memory repository
            return Task.CompletedTask;
        }

        public Task<Guid> DeleteUserAsync(Guid id)
        {
            _users.TryRemove(id, out _);
            return Task.FromResult(id);
        }
    }
}
