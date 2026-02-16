using Microsoft.EntityFrameworkCore;
using VaultEdge.Domain.Account;
using VaultEdge.Domain.User;

namespace VaultEdge.Infrastructure.Persistence
{
    public class VaultEdgeDbContext : DbContext
    {
        public VaultEdgeDbContext(DbContextOptions<VaultEdgeDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VaultEdgeDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
