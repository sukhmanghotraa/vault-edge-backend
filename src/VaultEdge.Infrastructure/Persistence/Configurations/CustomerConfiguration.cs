using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaultEdge.Domain.Customer;
using VaultEdge.Domain.ValueObjects;

namespace VaultEdge.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.TaxId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.IdentificationId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Nationality)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.DateOfBirth)
                .IsRequired();

            builder.Property(c => c.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .IsRequired(false);

            builder.Property(x => x.Email)
                .HasColumnName("Email")
                .HasMaxLength(255)
                .IsRequired()
                .HasConversion(
                    email => email.Value,
                    value => Email.Create(value)!)
                .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<Email>(
                    (e1, e2) => e1!.Value == e2!.Value,
                    e => e.Value.GetHashCode(),
                    e => Email.Create(e.Value)!));

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.PhoneNumber)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(20)
                .IsRequired()
                .HasConversion(
                    phone => phone.Value,
                    value => PhoneNumber.Create(value)!)
                .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<PhoneNumber>(
                    (p1, p2) => p1!.Value == p2!.Value,
                    p => p.Value.GetHashCode(),
                    p => PhoneNumber.Create(p.Value)!));

            builder.Property(x => x.Address)
                .HasColumnName("Address")
                .HasMaxLength(500)
                .IsRequired()
                .HasConversion(
                    address => address.Value,
                    value => Address.Create(value)!)
                .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<Address>(
                    (a1, a2) => a1!.Value == a2!.Value,
                    a => a.Value.GetHashCode(),
                    a => Address.Create(a.Value)!));

            builder.HasMany(c => c.Accounts)
                .WithOne()
                .HasForeignKey("CustomerId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
