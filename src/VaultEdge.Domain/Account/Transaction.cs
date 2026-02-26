using VaultEdge.Domain.Common.Models;
using VaultEdge.Domain.Enums;
using VaultEdge.Domain.ValueObjects;

namespace VaultEdge.Domain.Account
{
    public class Transaction: Entity
    {
        public TransactionType Type { get; private set; }
        public Money Amount { get; private set; }
        public string Description { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public Money BalanceAfter { get; private set; }
        public string? CounterpartyAccountNumber { get; private set; }
        public string? CounterpartyName { get; private set; }
        public Guid? TransferReferenceId { get; private set; }
        public string? ExternalReference { get; private set; }
        public Dictionary<string, string>? Metadata { get; private set; }

        private Transaction() { }

        internal static Transaction Create(
            TransactionType type,
            Money amount,
            Money balanceAfter,
            string description,
            string? counterpartyAccountNumber = null,
            string? counterpartyName = null,
            Guid? transferReferenceId = null,
            string? externalReference = null,
            Dictionary<string, string>? metadata = null)
        {
            if(amount == null)
                throw new ArgumentNullException(nameof(amount));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("Transaction description is required.", nameof(description));

            if(balanceAfter is null)
                throw new ArgumentNullException(nameof(balanceAfter));

            if(amount.Currency != balanceAfter.Currency)
                throw new InvalidOperationException("Amount and BalanceAfter must be in the same currency.");

            if(type == TransactionType.TransferOut || type == TransactionType.TransferIn)
            {
                if(string.IsNullOrWhiteSpace(counterpartyAccountNumber))
                    throw new ArgumentNullException("Counterparty account number is required for transfer transactions.", nameof(counterpartyAccountNumber));

                if (string.IsNullOrWhiteSpace(counterpartyName))
                    throw new ArgumentNullException("Counterparty name is required for transfer transactions.", nameof(counterpartyName));

                if(transferReferenceId is null)
                    throw new ArgumentException("Transfer reference ID is required for transfer transactions.", nameof(transferReferenceId));
            }

            return new Transaction
            {
                Id = Guid.NewGuid(),
                Type = type,
                Amount = amount,
                BalanceAfter = balanceAfter,
                Description = description,
                TransactionDate = DateTime.UtcNow,
                CounterpartyAccountNumber = counterpartyAccountNumber,
                CounterpartyName = counterpartyName,
                TransferReferenceId = transferReferenceId,
                ExternalReference = externalReference,
                Metadata = metadata
            };
        }

        public bool IsDebit() => Amount.IsNegative();

        public bool IsCredit() => Amount.IsPositive();

        public bool IsTransfer() => TransferReferenceId.HasValue;

        public string GetDisplayString()
        {
            var prefix = IsDebit() ? "-" : "+";
            var counterpartyInfo = !string.IsNullOrEmpty(CounterpartyName)
                ? $" ({CounterpartyName})"
                : "";

            return $"{prefix}{Amount.Abs()} - {Description}{counterpartyInfo}";
        }
    }
}
