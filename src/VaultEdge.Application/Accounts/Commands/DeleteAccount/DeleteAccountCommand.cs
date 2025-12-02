using VaultEdge.Application.Abstractions;

namespace VaultEdge.Application.Accounts.Commands.DeleteAccount
{
    public class DeleteAccountCommand : ICommand<Guid>
    {
        public Guid AccountId { get; set; }

        public DeleteAccountCommand(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}
