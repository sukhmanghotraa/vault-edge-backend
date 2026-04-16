using VaultEdge.Domain.Account;
using VaultEdge.Domain.Enums;
using VaultEdge.Domain.ValueObjects;

namespace VaultEdge.Application.Accounts.DTOs
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public AccountType AccountType { get; set; } = AccountType.Savings;
        public Currency Currency { get; set; } = Currency.USD;
        public Money Balance { get; set; } = Money.Zero(Currency.USD);
        public AccountStatus Status { get; set; } = AccountStatus.Pending;
        public IReadOnlyCollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public DateTime CreatedAt { get; set; }
    }
}
