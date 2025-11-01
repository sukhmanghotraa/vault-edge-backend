using System.Collections.Concurrent;
using VaultEdge.Domain.Entities;
using VaultEdge.Domain.Repositories;

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

        public Task<User?> GetByIdAsync(Guid id)
        {
            _users.TryGetValue(id, out var user);
            return Task.FromResult(user);
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            var user = _users.Values.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            return Task.FromResult(_users.Values.AsEnumerable());
        }

        public Task<User> CreateUserAsync(User user)
        {
            _users[user.Id] = user;
            return Task.FromResult(user);
        }

        public Task SaveChangesAsync()
        {
            // No-op for in-memory repository
            return Task.CompletedTask;
        }

        public Task DeleteUserAsync(Guid id)
        {
            _users.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}
