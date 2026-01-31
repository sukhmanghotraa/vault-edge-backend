using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Authentication
{
    public record AuthenticationResult(
        User User,
        string Token);
}
