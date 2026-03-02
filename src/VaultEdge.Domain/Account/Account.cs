using VaultEdge.Domain.Common.Models;
using VaultEdge.Domain.Enums;
using VaultEdge.Domain.ValueObjects;

namespace VaultEdge.Domain.Account
{
    public class Account : AggregateRoot
    {
        public string AccountNumber { get; private set; } = null!;
        public Guid CustomerId { get; private set; }

        public AccountType Type { get; private set; } = default!;
        public Currency Currency { get; private set; } = null!;
        public AccountStatus Status { get; private set; } = default!;

        private Money _balance = null!;
        public Money Balance => _balance;

        public Money OverdraftLimit { get; private set; } = null!;
        public Money? DailyWithdrawalLimit { get; private set; }
        public Money? MonthlyWithdrawalLimit { get; private set; }

        private readonly List<Transaction> _transactions = new();
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? ClosedAt { get; private set; }

        private Account() { }

        public static Account Create(
            Guid customerId,
            string accountNumber,
            AccountType type,
            Currency currency,
            Money? dailyWithdrawalLimit = null,
            Money? monthlyWithdrawalLimit = null
            )
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("Customer ID is required.", nameof(customerId));

            if(string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Account number is required.", nameof(accountNumber));

            if(currency is null)
                throw new ArgumentNullException(nameof(currency), "Currency is required");

            if(dailyWithdrawalLimit is null)
                throw new ArgumentNullException(nameof(dailyWithdrawalLimit), "Daily withdrawal limit is required");

            if(monthlyWithdrawalLimit is null)
                throw new ArgumentNullException(nameof(monthlyWithdrawalLimit), "Monthly withdrawal limit is required");

            var account = new Account
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                AccountNumber = accountNumber,
                Type = type,
                Currency = currency,
                Status = AccountStatus.Pending,
                _balance = Money.Zero(currency),
                CreatedAt = DateTime.UtcNow,
            };

            account.OverdraftLimit = type == AccountType.Checking
                ? Money.Create(500m, currency)
                : Money.Zero(currency);

            account.DailyWithdrawalLimit = dailyWithdrawalLimit;
            account.MonthlyWithdrawalLimit = monthlyWithdrawalLimit;

            return account;
        }

        public void Activate()
        {
            if (Status != AccountStatus.Pending)
                throw new InvalidOperationException($"Cannot activate account in {Status} status");

            Status = AccountStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Suspend(string reason)
        {
            if (Status == AccountStatus.Closed)
                throw new InvalidOperationException("Cannot suspend a closed account.");

            if(Status == AccountStatus.Pending)
                throw new InvalidOperationException("Cannot suspend a pending account.");

            if(string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Suspension reason is required", nameof(reason));

            Status = AccountStatus.Suspended;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Freeze(string reason)
        {
            if(Status == AccountStatus.Closed)
                throw new InvalidOperationException("Cannot freeze a closed account.");

            if(Status == AccountStatus.Pending)
                throw new InvalidOperationException("Cannot freeze a pending account.");

            if(string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Freeze reason is required", nameof(reason));

            Status = AccountStatus.Frozen;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reactivate()
        {
            if (Status != AccountStatus.Suspended && Status != AccountStatus.Frozen)
                throw new InvalidOperationException($"Cannot reactivate account in {Status} status.");

            Status = AccountStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Close()
        {
            if (Status == AccountStatus.Closed)
                throw new InvalidOperationException("Account is already closed.");

            if(!_balance.IsZero())
                throw new InvalidOperationException("Cannot close account with non-zero balance.");

            Status = AccountStatus.Closed;
            ClosedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deposit(Money amount, string description, string? externalRefernce = null, Dictionary<string, string>? metadata = null)
        {
            if (amount is null)
                throw new ArgumentException();

            if (amount.Currency != Currency)
                throw new InvalidOperationException($"Cannot deposit {amount.Currency.Code} into an account denominated in {Currency.Code}");

            if (!amount.IsPositive())
                throw new ArgumentException("Deposit amount must be positive.", nameof(amount));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Transaction description is required", nameof(description));

            if (Status == AccountStatus.Closed)
                throw new InvalidOperationException("Cannot deposit into a closed account.");

            if (Status == AccountStatus.Frozen)
                throw new InvalidOperationException("Cannot deposit into a frozen account.");

            _balance = _balance + amount;

            var transaction = Transaction.Create(
                type: TransactionType.Deposit,
                amount: amount,
                balanceAfter: _balance,
                description: description,
                externalReference: externalRefernce);

            _transactions.Add(transaction);
            UpdatedAt = DateTime.UtcNow;

            if (Status == AccountStatus.Pending)
            {
                Activate();
            }
        }

        public void Withdraw(Money amount, string description, string? externalRefernce = null, Dictionary<string, string>? metadata = null)
        {
            if (amount is null)
                throw new ArgumentException(nameof(amount));

            if (amount.Currency != Currency)
                throw new InvalidOperationException($"Cannot withdraw {amount.Currency.Code} into an account denominated in {Currency.Code}");

            if (!amount.IsPositive())
                throw new ArgumentException("Withdrawal amount must be positive", nameof(amount));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Transaction description is required", nameof(description));

            if (Status == AccountStatus.Closed)
                throw new InvalidOperationException("Cannot withdraw from a closed account.");

            if (Status == AccountStatus.Frozen)
                throw new InvalidOperationException("Cannot withdraw from a frozen account.");

            if (Status == AccountStatus.Suspended)
                throw new InvalidOperationException("Cannot withdraw from a suspended account.");

            if (Status != AccountStatus.Active)
                throw new InvalidOperationException($"Cannot withdraw from an account that is {Status} status");

            var balanceAfterWithdrawal = _balance - amount;
            var effectiveMinimunBalance = OverdraftLimit.Negate();
            
            if(balanceAfterWithdrawal < effectiveMinimunBalance)
            {
                var availableFunds = _balance + OverdraftLimit;
                throw new InvalidOperationException($"Insufficient funds. Available balance including overdraft is {availableFunds}");
            }

            if (DailyWithdrawalLimit is not null)
            {
                var todayWithdrawals = GetTodayWithdrawals();
                var totalWithdrawalsToday = todayWithdrawals + amount;

                if(totalWithdrawalsToday > DailyWithdrawalLimit)
                {
                    throw new InvalidOperationException(
                        $"Daily withdrawal limit exceeded. Limit: {DailyWithdrawalLimit}, " +
                        $"Already withdrawn today: {todayWithdrawals}, Requested: {amount}");
                }
            }

            if (MonthlyWithdrawalLimit is not null)
            {
                var thisMonthWithdrawals = GetThisMonthWithdrawals();
                var totalWithdrawalsThisMonth = thisMonthWithdrawals + amount;

                if (totalWithdrawalsThisMonth > MonthlyWithdrawalLimit)
                {
                    throw new InvalidOperationException(
                        $"Monthly withdrawal limit exceeded. Limit: {MonthlyWithdrawalLimit}, " +
                        $"Already withdrawn thid month: {thisMonthWithdrawals}, Requested: {amount}");
                }
            }

            _balance = balanceAfterWithdrawal;

            var transaction = Transaction.Create(
                type: TransactionType.Withdrawal,
                amount: amount.Negate(),
                balanceAfter: _balance,
                description: description,
                externalReference: externalRefernce);

            _transactions.Add(transaction);
            UpdatedAt = DateTime.UtcNow;
        }

        public Guid TransferOut(
            Money amount,
            string destinationAccountNumber,
            string destinationAccountHolderName,
            string description)
        {
            if (amount is null)
                throw new ArgumentNullException(nameof(amount));

            if (amount.Currency != Currency)
                throw new InvalidOperationException(
                    $"Cannot transfer {amount.Currency.Code} from an account denominated in {Currency.Code}");

            if (!amount.IsPositive())
                throw new ArgumentException("Transfer amount must be positive.", nameof(amount));

            if (string.IsNullOrWhiteSpace(destinationAccountNumber))
                throw new ArgumentException("Destination account number is required.", nameof(destinationAccountNumber));

            if (string.IsNullOrWhiteSpace(destinationAccountHolderName))
                throw new ArgumentException("Destination account holder name is required.", nameof(destinationAccountHolderName));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Transaction description is required.", nameof(description));

            if(Status != AccountStatus.Active)
                throw new InvalidOperationException($"Cannot transfer from an account that is {Status} status.");

            var balanceAfterTransfer = _balance - amount;
            var effectiveMinimunBalance = OverdraftLimit.Negate();

            if (balanceAfterTransfer < effectiveMinimunBalance)
            {
                var availableFund = _balance + OverdraftLimit;
                throw new InvalidOperationException($"Insufficient funds. Available balance including overdraft is {availableFund}");
            }

            if (DailyWithdrawalLimit is not null)
            {
                var todayWithdrawals = GetTodayWithdrawals();

                if (todayWithdrawals + amount > DailyWithdrawalLimit)
                {
                    throw new InvalidOperationException("Daily withdrawal limit exceeded by this transafer");
                }
            }

            if (MonthlyWithdrawalLimit is not null)
            {
                var thisMonthWithdrawals = GetThisMonthWithdrawals();

                if (thisMonthWithdrawals + amount > MonthlyWithdrawalLimit)
                {
                    throw new InvalidOperationException(
                        $"Monthly withdrawal limit exceeded. Limit: {MonthlyWithdrawalLimit}, " +
                        $"Already withdrawn thid month: {thisMonthWithdrawals}, Requested: {amount}");
                }
            }

            var transferReferenceId = Guid.NewGuid();

            _balance = balanceAfterTransfer;

            var transaction = Transaction.Create(
                type: TransactionType.TransferOut,
                amount: amount.Negate(),
                balanceAfter: _balance,
                 description: description,
                 counterpartyAccountNumber: destinationAccountNumber,
                 counterpartyName: destinationAccountHolderName,
                 transferReferenceId: transferReferenceId);

            _transactions.Add(transaction);
            UpdatedAt = DateTime.UtcNow;

            return transferReferenceId;
        }

        public void TransferIn(
            Money amount,
            string sourceAccountNumber,
            string sourceAccountHolderName,
            string description,
            Guid transferReferenceId)
        {
            if (amount is null)
                throw new ArgumentNullException(nameof(amount));

            if (amount.Currency != Currency)
                throw new InvalidOperationException(
                    $"Cannot transfer {amount.Currency.Code} into an account denominated in {Currency.Code}");

            if (!amount.IsPositive())
                throw new ArgumentException("Transfer amount must be positive.", nameof(amount));

            if (string.IsNullOrWhiteSpace(sourceAccountNumber))
                throw new ArgumentNullException("Source account number is required.", nameof(sourceAccountNumber));

            if (string.IsNullOrWhiteSpace(sourceAccountHolderName))
                throw new ArgumentNullException("Source account holder name is required.", nameof(sourceAccountHolderName));
            
            if (string.IsNullOrWhiteSpace(description))
                 throw new ArgumentNullException("Transaction description is required.", nameof(description));

            if (Status == AccountStatus.Closed)
                throw new InvalidOperationException("Cannot transfer into a closed an account");

            if (Status == AccountStatus.Frozen)
                throw new InvalidOperationException("Cannot transfer into a frozen an account");

            _balance = _balance + amount;

            var transaction = Transaction.Create(
                type: TransactionType.TransferIn,
                amount: amount,
                balanceAfter: _balance,
                description: description,
                counterpartyAccountNumber: sourceAccountHolderName,
                counterpartyName: sourceAccountHolderName,
                transferReferenceId: transferReferenceId);

            _transactions.Add(transaction);
            UpdatedAt = DateTime.UtcNow;

            if (Status == AccountStatus.Pending)
            {
                Activate();
            }
        }

        public void ReverseTransaction(Guid originalTransactionId, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentNullException("Reversal reason is required.", nameof(reason));

            var originalTransaction = _transactions.FirstOrDefault(x => x.Id == originalTransactionId);
            if (originalTransaction is null)
                throw new ArgumentNullException("Original transaction not found.", nameof(originalTransaction));

            if (Status == AccountStatus.Closed)
                throw new InvalidOperationException("Cannot reverse a transactions on a closed account.");

            var reversalAmount = originalTransaction.Amount.Negate();
            _balance = _balance + reversalAmount;

            var reversalDescription = $"Reversal of transaction {originalTransactionId}: {reason}";

            var reversalTransaction = Transaction.Create(
                type: TransactionType.Reversal,
                amount: reversalAmount,
                balanceAfter: _balance,
                description: reversalDescription,
                externalReference: $"REVERSAL-{originalTransactionId}");

            _transactions.Add(reversalTransaction);
            UpdatedAt = DateTime.UtcNow;
        }

        private Money GetTodayWithdrawals()
        {
            var today = DateTime.UtcNow.Date;
            var todayTransactions = _transactions
                .Where(x => x.Type == TransactionType.Withdrawal || x.Type == TransactionType.TransferOut)
                .ToList();

            return todayTransactions
                .Select(x => x.Amount.Abs())
                .Aggregate(Money.Zero(Currency), (sum, amount) => sum + amount);
        }

        private Money GetThisMonthWithdrawals()
        {
            var now = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);

            var thisMonthTransaction = _transactions
                .Where(x => x.TransactionDate >= firstDayOfMonth)
                .Where(x => x.Type == TransactionType.Withdrawal || x.Type == TransactionType.TransferOut)
                .ToList();

            return thisMonthTransaction
                .Select(x => x.Amount.Abs())
                .Aggregate(Money.Zero(Currency), (sum, amount) => sum + amount);
        }

        public IEnumerable<Transaction> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _transactions
                .Where(x => x.TransactionDate >= startDate && x.TransactionDate <= endDate)
                .OrderBy(x => x.TransactionDate)
                .ToList();
        }

        public IEnumerable<Transaction> GetRecentTransactions(int count)
        {
            return _transactions
                .OrderByDescending(x => x.TransactionDate)
                .Take(count)
                .ToList();
        }

        public bool HasSufficientFunds(Money amount)
        {
            if(amount.Currency != Currency)
                throw new InvalidOperationException($"Cannot compare funds for different currencies. Account currency: {Currency.Code}, Amount currency: {amount.Currency.Code}");

            var availableFunds = _balance + OverdraftLimit;
            return availableFunds >= amount;
        }

        public Money GetAvailAblefunds()
        {
            return _balance + OverdraftLimit;
        }

        public bool IsOverdrawn()
        {
            return _balance < OverdraftLimit;
        }
    }
}
