using VaultEdge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using VaultEdge.Domain.Repositories;

namespace VaultEdge.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly VaultEdgeDbContext _context;

        public AccountRepository(VaultEdgeDbContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetAccountByIdAsync(Guid id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            account.Id = Guid.NewGuid();
            account.Balance = 0; // new accounts start with 0 balance

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<Account?> DeleteAccountAsync(Guid id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return null;
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<Account?> DepositAsync(Guid accountId, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) return null;

            account.Balance += amount;
            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<Account?> WithdrawAsync(Guid accountId, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null || account.Balance < amount) return null;

            account.Balance -= amount;
            await _context.SaveChangesAsync();

            return account;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


        public async Task AddAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task<Account?> GetByIdAsync(Guid id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

    }
}
