using Microsoft.EntityFrameworkCore;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Account;

namespace VaultEdge.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly VaultEdgeDbContext _context;

        public AccountRepository(VaultEdgeDbContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber, cancellationToken);
        }

        public async Task<List<Account>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            // Current: All accounts with all transaction, which is currently a lot of data
            // TODO: Add pagination
            return await _context.Accounts
                .Include(a => a.Transactions)
                .Where(a => a.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Account>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Accounts
                .Include(a => a.Transactions)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
        {
            await _context.Accounts.AddAsync(account, cancellationToken);
        }

        public void Update(Account account)
        {
            _context.Accounts.Update(account);
        }

        public void Delete(Account account)
        {
            _context.Accounts.Remove(account);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync();
        }
    }
}
