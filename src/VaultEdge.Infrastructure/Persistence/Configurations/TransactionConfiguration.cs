using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaultEdge.Domain.Account;
using VaultEdge.Domain.ValueObjects;

namespace VaultEdge.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");
            builder.HasKey(t => t.Id);

            builder.Property<Guid>("AccountId")
                .IsRequired();

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Type)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            builder.OwnsOne(t => t.Amount, amount =>
            {
                amount.Property(m => m.Amount)
                    .HasColumnName("Amount")
                    .IsRequired()
                    .HasPrecision(18, 2);

                amount.Property(m => m.Currency)
                    .HasConversion(c => c.Code, code => Currency.FromCode(code)!)
                    .HasColumnName("Currency")
                    .IsRequired()
                    .HasMaxLength(3);
            });

            builder.OwnsOne(t => t.BalanceAfter, balance =>
            {
                balance.Property(m => m.Amount)
                    .HasColumnName("BalanceAfter")
                    .IsRequired()
                    .HasPrecision(18, 2);

                balance.Property(m => m.Currency)
                    .HasConversion(c => c.Code, code => Currency.FromCode(code)!)
                    .HasColumnName("BalanceAfterCurrency")
                    .IsRequired()
                    .HasMaxLength(3);
            });

            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(t => t.TransactionDate)
                .IsRequired();

            builder.Property(t => t.CounterpartyAccountNumber)
                .HasMaxLength(20);

            builder.Property(t => t.CounterpartyName)
                .HasMaxLength(200);

            builder.Property(t => t.TransferReferenceId);

            builder.Property(t => t.ExternalReference)
                .HasMaxLength(100);

            builder.Property(t => t.Metadata)
                .HasColumnType("json");
        }
    }
}
