using VaultEdge.Domain.User;

namespace VaultEdge.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
