using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using System.Linq;
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

            var metadataComparer = new ValueComparer<Dictionary<string, string>?>(
                (dict1, dict2) =>
                    dict1 == null && dict2 == null
                        ? true
                        : dict1 != null && dict2 != null
                          && dict1.Count == dict2.Count
                          && dict1.All(kvp => dict2.ContainsKey(kvp.Key) && dict2[kvp.Key] == kvp.Value),

                dict => dict == null
                    ? 0
                    : dict.Aggregate(0, (hash, kvp) => hash ^ (kvp.Key.GetHashCode() ^ kvp.Value.GetHashCode())),

                dict => dict == null ? null : new Dictionary<string, string>(dict)
            );

            builder.Property(t => t.Metadata)
                .HasConversion(
                    dictionary => dictionary == null
                        ? null
                        : JsonSerializer.Serialize(dictionary, (JsonSerializerOptions?)null),
                    json => string.IsNullOrEmpty(json)
                        ? null
                        : JsonSerializer.Deserialize<Dictionary<string, string>>(json, (JsonSerializerOptions?)null),
                    metadataComparer)
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);
        }
    }
}
