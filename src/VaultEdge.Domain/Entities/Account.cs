
namespace VaultEdge.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; private set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public Guid UserId { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; set; }


        private Account() { }

        public Account(string accountNumber, Guid userId, decimal balance)
        {
            Id = Guid.NewGuid();
            AccountNumber = accountNumber;
            Balance = balance;
            UserId = userId;

            CreatedAt = DateTime.UtcNow;
            AccountType = AccountType.Savings;
        }
    }



    public enum AccountType
    {
        Savings,
        Deposit,
        Trading,
        Insurance
    }
}
