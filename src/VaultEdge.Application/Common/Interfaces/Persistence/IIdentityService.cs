namespace VaultEdge.Application.Common.Interfaces.Persistence
{
    public interface IIdentityService
    {
        Task<IdentityVerificationResult> AuthenticateAsync(string email, string password);
        Task<IdentityResult> CreateUserAsync(Guid customerUd, string email, string password);
        Task<IdentityResult> ChangePasswordAsync(Guid customerUd, string currentPassword, string newPassword);
        Task<IdentityResult> ConfirmEmailAsync(Guid customerUd, string token);
        Task<bool> IsLockedOutAsync(Guid customerUd);
        Task<string> GetSecurityStampAsync(Guid customerUd);
    }

    public class IdentityVerificationResult
    {
        public bool Succeeded { get; init; }
        public Guid CustomerId { get; init; }
        public string SecurityStamp { get; init; }
        public string Email { get; init; }
        public string ErrorMessage { get; init; }
        public bool RequiresTwoFactor { get; init; }

        public static IdentityVerificationResult Success(Guid customerId, string email, string securityStamp) =>
            new()
            {
                Succeeded = true,
                CustomerId = customerId,
                Email = email,
                SecurityStamp = securityStamp,
            };

        public static IdentityVerificationResult Failure(string errorMessage)
            => new()
            {
                Succeeded = false,
                ErrorMessage = errorMessage,
            };

        public static IdentityVerificationResult TwoFactorRequired(Guid customerId)
            => new()
            {
                Succeeded = false,
                RequiresTwoFactor = true,
                CustomerId = customerId,
            };
    }

    public class IdentityResult
    {
        public bool Succeeded { get; init; }
        public IEnumerable<string> Errors { get; init; } = new List<string>();
        public static IdentityResult Success() 
            => new() { Succeeded = true };
        public static IdentityResult Failure(IEnumerable<string> errors) 
            => new() { Succeeded = false, Errors = errors };

    }
}
