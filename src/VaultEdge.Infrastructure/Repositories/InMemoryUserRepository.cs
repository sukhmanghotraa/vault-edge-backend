using System.Collections.Concurrent;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Customer;

namespace VaultEdge.Infrastructure.Persistence.Repositories
{
    public class InMemoryUserRepository : ICustomerRepository
    {
        private static readonly ConcurrentDictionary<Guid, Customer> _users = new();

        public Task AddAsync(Customer customer)
        {
            _users[customer.Id] = customer;
            return Task.CompletedTask;
        }

        public Task<Customer> CreateCustomerAsync(Customer customer)
        {
            _users[customer.Id] = customer;
            return Task.FromResult(customer);
        }

        public Task<Customer?> GetByIdAsync(Guid id)
        {
            _users.TryGetValue(id, out var customer);
            return Task.FromResult(customer);
        }

        public Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var customer = _users.Values.FirstOrDefault(u => u.Email == email);
            return Task.FromResult(customer);
        }

        public Task<Customer?> GetByCustomerIdAsync(Guid customerId)
        {
            var customer = _users.Values.FirstOrDefault(u => u.Id == customerId);
            return Task.FromResult(customer);
        }

        public Task<IEnumerable<Customer>> GetAllAsync()
        {
            return Task.FromResult(_users.Values.AsEnumerable());
        }

        public Task SaveChangesAsync()
        {
            // No-op for in-memory repository
            return Task.CompletedTask;
        }

        public Task<Guid> DeleteCustomerAsync(Guid id)
        {
            _users.TryRemove(id, out _);
            return Task.FromResult(id);
        }
    }
}
