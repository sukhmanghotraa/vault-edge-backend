using VaultEdge.Domain.Customer;

namespace VaultEdge.Application.Authentication.Common
{
    public record AuthenticationResult(
        Customer Customer,
        string AccessToken,
        string RefreshToken);
}
