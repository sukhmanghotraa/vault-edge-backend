using MediatR;

namespace VaultEdge.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<int>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public decimal Balance { get; set; }
    }
}
