using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaultEdge.Domain.Account;
using VaultEdge.Domain.ValueObjects;

namespace VaultEdge.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.AccountNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(a => a.AccountNumber)
                .IsUnique();

            builder.Property(a => a.CustomerId)
                .IsRequired();

            builder.Property(a => a.Type)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(a => a.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(a => a.CreatedAt).IsRequired();
            builder.Property(a => a.UpdatedAt).IsRequired(false);
            builder.Property(a => a.ClosedAt).IsRequired(false);

            builder.Property(a => a.Currency)
                .HasConversion(
                    currency => currency.Code,
                    code => Currency.FromCode(code)!)
                .HasColumnName("CurrencyCode")
                .IsRequired()
                .HasMaxLength(3);

            builder.OwnsOne(a => a.Balance, balance =>
            {
                balance.Property(b => b.Amount)
                    .HasColumnName("BalanceAmount")
                    .IsRequired()
                    .HasPrecision(18, 2);

                balance.Property(b => b.Currency)
                    .HasConversion(
                        currency => currency.Code,
                        code => Currency.FromCode(code)!)
                    .HasColumnName("BalanceCurrency")
                    .IsRequired()
                    .HasMaxLength(3);
            });

            builder.OwnsOne(a => a.OverdraftLimit, limit =>
            {
                limit.Property(m => m.Amount)
                    .HasColumnName("OverdraftLimitAmount")
                    .HasPrecision(18, 2);

                limit.Property(m => m.Currency)
                    .HasConversion(c => c.Code, code => Currency.FromCode(code)!)
                    .HasColumnName("OverdraftLimitCurrency")
                    .HasMaxLength(3);
            });

            builder.OwnsOne(a => a.DailyWithdrawalLimit, limit =>
            {
                limit.Property(m => m.Amount)
                    .HasColumnName("DailyWithdrawalLimit")
                    .HasPrecision(18, 2);

                limit.Property(m => m.Currency)
                    .HasConversion(c => c.Code, code => Currency.FromCode(code)!)
                    .HasColumnName("DailyWithdrawalLimitCurrency")
                    .HasMaxLength(3);
            });

            builder.OwnsOne(a => a.MonthlyWithdrawalLimit, limit =>
            {
                limit.Property(m => m.Amount)
                    .HasColumnName("MonthlyWithdrawalLimit")
                    .HasPrecision(18, 2);

                limit.Property(m => m.Currency)
                    .HasConversion(c => c.Code, code => Currency.FromCode(code)!)
                    .HasColumnName("MonthlyWithdrawalLimitCurrency")
                    .HasMaxLength(3);
            });

            builder.HasMany<Transaction>("_transactions")
                .WithOne()
                .HasForeignKey("AccountId")
                .OnDelete(DeleteBehavior.Cascade); // If account is deleted, also delete related transactions
        }
    }
}
