using ErrorOr;
using MediatR;
using VaultEdge.Application.Authentication.Common;

namespace VaultEdge.Application.Authentication.Commands.Signup
{
    public record SignupCommand(
        string FirstName, 
        string LastName, 
        string PasswordHash, 
        DateTime DateOfBirth, 
        string TaxId, 
        string IdentificationId, 
        string Nationality, 
        string Email, 
        string PhoneNumber, 
        string Address): IRequest<ErrorOr<AuthenticationResult>>;
}