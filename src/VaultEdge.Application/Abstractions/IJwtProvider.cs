using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Abstractions
{
    public interface IJwtProvider
    {
        string Generate(User user);
    }
}
