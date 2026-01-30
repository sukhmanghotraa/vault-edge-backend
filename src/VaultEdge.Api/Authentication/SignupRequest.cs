namespace VaultEdge.Api.Authentication
{
    public record SignupRequest(
        string FirstName,
        string LastName,
        string Email,
        string Password);
}
