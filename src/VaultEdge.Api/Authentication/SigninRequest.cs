namespace VaultEdge.Api.Authentication
{
    public record SigninRequest(
        string Email,
        string Password);
}
