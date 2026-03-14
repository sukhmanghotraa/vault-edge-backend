using VaultEdge.Domain.Account;
using VaultEdge.Domain.Enums;

namespace VaultEdge.Application.Accounts.DTOs
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public AccountType AccountType { get; set; } = AccountType.Savings;
        public IReadOnlyCollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public DateTime CreatedAt { get; set; }
    }
}
