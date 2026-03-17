using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VaultEdge.Application.Common.Interfaces.Persistence;
using IdentityResult = VaultEdge.Application.Common.Interfaces.Persistence.IdentityResult;

namespace VaultEdge.Infrastructure.Identity
{
    public class IdentityService: IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityVerificationResult> AuthenticateAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return IdentityVerificationResult.Failure("Invalid email or password");
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return IdentityVerificationResult.Failure("Account is locked due to multiple failed login attempts");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
            {
                await _userManager.AccessFailedAsync(user);
                return IdentityVerificationResult.Failure("Invalid email or password");
            }


            if (user.TwoFactorEnabled)
            {
                return IdentityVerificationResult.TwoFactorRequired(user.CustomerId);
            }

            await _userManager.ResetAccessFailedCountAsync(user);

            user.LastLoginDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return IdentityVerificationResult.Success(user.CustomerId, user.Email, user.SecurityStamp);
        }

        public async Task<IdentityResult> CreateUserAsync(Guid customerId, string email, string password)
        {
            var user = new ApplicationUser
            {
                CustomerId = customerId,
                UserName = email,
                Email = email,
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return IdentityResult.Failure(result.Errors.Select(e => e.Description).ToArray());
            }

            return IdentityResult.Success();
        }

        public async Task<IdentityResult> ChangePasswordAsync(Guid customerId, string currentPassword, string newPassword)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.CustomerId == customerId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            return IdentityResult.Success();
        }

        public async Task<IdentityResult> ConfirmEmailAsync(Guid customerId, string token)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.CustomerId == customerId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            return IdentityResult.Success();
        }

        public async Task<bool> IsLockedOutAsync(Guid customerId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.CustomerId == customerId);
            if (user == null) return false;

            return await _userManager.IsLockedOutAsync(user);
        }

        public async Task<string> GetSecurityStampAsync(Guid customerId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.CustomerId == customerId);
            return user?.SecurityStamp ?? string.Empty;
        }
    }
}
