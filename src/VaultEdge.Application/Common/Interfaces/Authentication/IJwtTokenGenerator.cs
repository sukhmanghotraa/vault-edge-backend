using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
