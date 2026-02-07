using ErrorOr;
using MediatR;
using VaultEdge.Application.Authentication.Common;

namespace VaultEdge.Application.Authentication.Commands.Signup
{
    public record SignupCommand(
        string firstName, 
        string lastName, 
        string passwordHash, 
        DateTime dateOfBirth, 
        string taxId, 
        string identificationId, 
        string nationality, 
        string email, 
        string phoneNumber, 
        string address): IRequest<ErrorOr<AuthenticationResult>>;
}