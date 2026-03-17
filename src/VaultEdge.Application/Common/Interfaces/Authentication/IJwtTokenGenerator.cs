using VaultEdge.Domain.Customer;

namespace VaultEdge.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccesssToken(Customer user, string? securityStamp = null);
        string GenerateRefreshToken();
    }
}
