using VaultEdge.Domain.Customer;

namespace VaultEdge.Application.Authentication.Common
{
    public record AuthenticationResult(
        Customer User,
        string Token);
}
