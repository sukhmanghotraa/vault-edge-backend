using VaultEdge.Application.Abstractions;

namespace VaultEdge.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : ICommand<Guid>
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required DateTime DateOfBirth { get; init; }
        public required string TaxId { get; init; }
        public required string IdentificationId { get; init; }
        public required string Nationality { get; init; }
        public required string Email { get; init; }
        public required string PhoneNumber { get; init; }
        public required string Address { get; init; }
    }
}
