namespace VaultEdge.Application.Authentication
{
    public interface IAuthenticationService
    {
        AuthenticationResult Signup(string firstName, string lastName, string email, string password);

        AuthenticationResult Signin(string email, string password);
    }
}
