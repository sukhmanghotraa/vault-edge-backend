using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultEdge.Domain.Entities;
using VaultEdge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace VaultEdge.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly VaultEdgeDbContext _context;

        public AccountService(VaultEdgeDbContext context)
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

        public async Task<bool> DeleteAccountAsync(Guid id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return false;

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
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
    }
}
