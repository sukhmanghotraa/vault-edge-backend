using VaultEdge.Domain.User;

namespace VaultEdge.Application.Authentication.Common
{
    public record AuthenticationResult(
        User User,
        string Token);
}
