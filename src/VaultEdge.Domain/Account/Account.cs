namespace VaultEdge.Domain.Account
{
    public class Account
    {
        public Guid Id { get; private set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public Guid UserId { get; set; }
        public AccountType AccountType { get; set; }
        public AccountStatus AccountStatus { get; set; }
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
            AccountStatus = AccountStatus.Active;
        }
    }

    public enum AccountStatus
    {
        Active,
        Inactive,
        Closed,
        Suspended
    }

    public enum AccountType
    {
        Savings,
        Deposit,
        Trading,
        Insurance
    }
}
