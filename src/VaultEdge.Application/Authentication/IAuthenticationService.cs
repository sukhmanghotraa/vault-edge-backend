namespace VaultEdge.Application.Authentication
{
    public interface IAuthenticationService
    {
        AuthenticationResult Signup(string firstName, string lastName, string passwordHash, DateTime dateOfBirth, string taxId, string identificationId, string nationality, string email, string phoneNumber, string address);

        AuthenticationResult Signin(string email, string password);
    }
}
