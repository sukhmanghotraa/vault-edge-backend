using VaultEdge.Application.Abstractions;
using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Accounts.Commands.CreateAccount
{
    public record CreateAccountCommand : ICommand<Guid>
    {
        public required Guid UserId { get; init; }
        public decimal Balance { get; init; }
        public required AccountType AccountType { get; init; }
        public required string AccountNumber { get; init; }

    }
}
