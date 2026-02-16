using System.Collections.Concurrent;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Account;

namespace VaultEdge.Infrastructure.Persistence.Repositories
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        private static readonly ConcurrentDictionary<Guid, Account> _accounts = new();

        public Task AddAsync(Account account)
        {
            _accounts[account.Id] = account;
            return Task.CompletedTask;
        }

        public Task<Account> CreateAccountAsync(Account account)
        {
            _accounts[account.Id] = account;
            return Task.FromResult(account);
        }

        public Task<Account?> GetByIdAsync(Guid id)
        {
            _accounts.TryGetValue(id, out var account);
            return Task.FromResult(account);
        }

        public Task<IEnumerable<Account>> GetAllAsync()
        {
            return Task.FromResult(_accounts.Values.AsEnumerable());
        }

        public Task<Guid> DeleteAccountAsync(Guid id)
        {
            _accounts.TryRemove(id, out _);
            return Task.FromResult(id);
        }

        public Task SaveChangesAsync()
        {
            // No-op for in-memory store
            return Task.CompletedTask;
        }
    }
}
