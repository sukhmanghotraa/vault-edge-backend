using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserName(Guid userId);
        Task<string> GetUserNameAsync(Guid userId);
        Task<User> GetUserById(Guid userId);
        Task<bool> isUserActive(Guid userId);
        Task<User> IsUserActiveAsync(Guid userId);
        Task<IEnumerable<Guid>> GetUserRoleAsync(Guid userId);
        Task<User> ValidateUserCredentialsAsync(string username, string password);
        Task<Guid> CreateUserAsync(string username, string password, IEnumerable<Guid> roleIds);
        Task<Guid> DeleteUserAsync(Guid userId);
        Task<User> ActivateUserAsync(Guid userId);
        Task<User> DeactiveUserAsync(Guid userId);
        Task<User> ChangeUserPasswordAsync(Guid userId, string newPassword);
    }
}
