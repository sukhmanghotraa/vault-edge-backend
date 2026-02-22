using VaultEdge.Domain.Customer;

namespace VaultEdge.Application.Repositories
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
        Task<Customer?> GetByIdAsync(Guid id);
        Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Customer?> GetByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task SaveChangesAsync();
        Task<Guid> DeleteCustomerAsync(Guid id);
    }
}
