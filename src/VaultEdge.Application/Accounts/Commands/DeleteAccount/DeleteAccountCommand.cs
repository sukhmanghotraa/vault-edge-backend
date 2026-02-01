using VaultEdge.Application.Abstractions;

namespace VaultEdge.Application.Accounts.Commands.DeleteAccount
{
    public record DeleteAccountCommand(Guid AccountId) : ICommand<Guid>;
}
