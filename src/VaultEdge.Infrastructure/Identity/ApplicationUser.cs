using Microsoft.AspNetCore.Identity;

namespace VaultEdge.Infrastructure.Identity
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        public Guid CustomerId { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
