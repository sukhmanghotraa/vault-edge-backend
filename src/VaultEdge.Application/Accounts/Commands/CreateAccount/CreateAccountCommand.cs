using VaultEdge.Application.Abstractions;
using VaultEdge.Domain.Account;

namespace VaultEdge.Application.Accounts.Commands.CreateAccount
{
    public record CreateAccountCommand : ICommand<Guid>
    {
        public required Guid CustomerId { get; init; }
        public required string AccountNumber { get; init; }
        public required string Type { get; init; }
        public required string CurrencyCode { get; init; }

        public IReadOnlyCollection<Transaction> Transactions{ get; init; } = new List<Transaction>();

        public decimal? DailyWithdrawalLimit { get; init; }
        public string? DailyWithdrawalLimitCurrency { get; init; }

        public decimal? MonthlyWithdrawalLimit { get; init; }
        public string? MonthlyWithdrawalLimitCurrency { get; init; }
    }
}
