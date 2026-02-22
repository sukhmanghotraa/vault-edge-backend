using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VaultEdge.Domain.Account;
using VaultEdge.Domain.Customer;
using VaultEdge.Infrastructure.Identity;

namespace VaultEdge.Infrastructure.Persistence
{
    public class VaultEdgeDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public VaultEdgeDbContext(DbContextOptions<VaultEdgeDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("ApplicationUsers");
                entity.HasOne<Customer>()
                    .WithOne()
                    .HasForeignKey<ApplicationUser>(x => x.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(x => x.CustomerId)
                    .IsUnique();
            });

            modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VaultEdgeDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
