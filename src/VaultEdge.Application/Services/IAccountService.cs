using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Services
{
    public interface IAccountService
    {
        Task<Account?> GetAccountByIdAsync(Guid id);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account> CreateAccountAsync(Account account);
        Task<bool> DeleteAccountAsync(Guid id);
        Task<Account?> DepositAsync(Guid accountId, decimal amount);
        Task<Account?> WithdrawAsync(Guid accountId, decimal amount);
    }
}
