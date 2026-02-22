using Microsoft.EntityFrameworkCore;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Customer;

namespace VaultEdge.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly VaultEdgeDbContext _context;

        public CustomerRepository(VaultEdgeDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> GetByIdAsync(Guid id) =>
            await _context.Customers.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
            await _context.Customers.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        public async Task<Customer?> GetByCustomerIdAsync(Guid customerId) =>
            await _context.Customers.FirstOrDefaultAsync(u => u.Id == customerId); 

        public async Task<IEnumerable<Customer>> GetAllAsync() =>
            await _context.Customers.ToListAsync();

        public async Task<Guid> DeleteCustomerAsync(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                throw new Exception("Account not found");
            }
            
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            
            return customer.Id;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
