using VaultEdge.Domain.Customer;

namespace VaultEdge.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Customer user, string? securityStamp = null);
    }
}
