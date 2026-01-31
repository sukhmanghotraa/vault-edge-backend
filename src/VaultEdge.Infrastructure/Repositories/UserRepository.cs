using Microsoft.EntityFrameworkCore;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Entities;

namespace VaultEdge.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly VaultEdgeDbContext _context;

        public UserRepository(VaultEdgeDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        public async Task<User?> GetByCustomerIdAsync(Guid customerId) =>
            await _context.Users.FirstOrDefaultAsync(u => u.CustomerId == customerId); 

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _context.Users.ToListAsync();

        public async Task<Guid> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new Exception("Account not found");
            }
            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            
            return user.Id;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
