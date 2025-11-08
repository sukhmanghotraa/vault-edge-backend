using MediatR;
using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<Guid>
    {
        public required Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public required AccountType AccountType { get; set; }
        public required string AccountNumber { get; set; }

    }
}
