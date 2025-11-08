using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Accounts.DTOs
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public Guid UserId { get; set; }
        public AccountType AccountType { get; set; } = AccountType.Savings;
        public DateTime CreatedAt { get; set; }
    }
}
