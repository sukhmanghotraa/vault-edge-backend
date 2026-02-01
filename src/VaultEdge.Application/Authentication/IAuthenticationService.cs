using ErrorOr;

namespace VaultEdge.Application.Authentication
{
    public interface IAuthenticationService
    {
        ErrorOr<AuthenticationResult> Signup(string firstName, string lastName, string passwordHash, DateTime dateOfBirth, string taxId, string identificationId, string nationality, string email, string phoneNumber, string address);

        ErrorOr<AuthenticationResult> Signin(string email, string password);
    }
}
