using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Authentication.Common
{
    public record AuthenticationResult(
        User User,
        string Token);
}
