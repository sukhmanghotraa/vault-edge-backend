using MediatR;
using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public decimal Balance { get; set; }
        public required Guid UserId { get; set; }
        public required AccountType AccountType { get; set; }
    }
}
