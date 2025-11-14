using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaultEdge.Domain.Entities;

namespace VaultEdge.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            // Set the primary key
            builder.HasKey(a => a.Id);

            // Configure the 'Balance' property to fix the warning
            // This sets the SQL column type to DECIMAL(18, 2), which is standard for currency
            builder.Property(a => a.Balance)
                .IsRequired()
                .HasPrecision(18, 2); // 18 total digits, 2 decimal places

            // Configure the relationship between Account and User
            // Assuming Account has a UserId foreign key property
            //builder.HasOne(a => a.User)
            //    .WithMany(u => u.Accounts)
            //    .HasForeignKey(a => a.UserId);

            // Add any other specific configuration here (e.g., indexes, default values)
        }
    }
}
