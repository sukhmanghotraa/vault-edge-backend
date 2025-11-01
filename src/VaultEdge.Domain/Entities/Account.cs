
namespace VaultEdge.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public Guid UserId { get; set; }
        public AccountType AccountType { get; set; }
    }

    public enum AccountType
    {
        PedningVerification,
        Active,
        Suspended,
        Closed
    }
}
