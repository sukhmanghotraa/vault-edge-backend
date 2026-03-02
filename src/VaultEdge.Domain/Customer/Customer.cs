using VaultEdge.Domain.Account;
using VaultEdge.Domain.Common.Models;
using VaultEdge.Domain.Enums;
using VaultEdge.Domain.ValueObjects;

namespace VaultEdge.Domain.Customer
{
    public class Customer : AggregateRoot
    {
        public  string FirstName { get; private set; } = string.Empty;
        public  string LastName { get; private set; } = string.Empty;
        public DateTime DateOfBirth { get; private set; }

        public string TaxId { get; private set; } = string.Empty;
        public string IdentificationId { get; private set; } = string.Empty;
        public  string Nationality { get; private set; } = string.Empty;

        private Email _email = null!;
        private PhoneNumber _phoneNumber = null!;
        private Address _address = null!;

        public Email Email => _email;
        public PhoneNumber PhoneNumber => _phoneNumber;
        public Address Address => _address;

        public  CustomerStatus Status{ get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private readonly List<Account.Account> _accounts = new();

        public IReadOnlyCollection<Account.Account> Accounts => _accounts.AsReadOnly();

        private Customer() { }

        public static Customer Create(
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string taxId,
            string identificationId,
            string nationality,
            Email email,
            PhoneNumber phoneNumber,
            Address address)
        {
            if (string.IsNullOrWhiteSpace(firstName)) 
                throw new ArgumentException("First name is required", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName)) 
                throw new ArgumentException("Email is requred", nameof(lastName));
            if (dateOfBirth > DateTime.UtcNow.AddYears(-18))
                throw new ArgumentException("Customer must be at least 18 years old", nameof(dateOfBirth));
            if (string.IsNullOrWhiteSpace(taxId))
                throw new ArgumentException("Tax ID is required", nameof(taxId));
            if (string.IsNullOrWhiteSpace(identificationId))
                throw new ArgumentException("Identification ID is required", nameof(identificationId));
            if (string.IsNullOrWhiteSpace(nationality))
                throw new ArgumentException("Nationality is required", nameof(nationality));
            if (email is null)
                throw new ArgumentNullException(nameof(email), "Email is required");
            if (phoneNumber is null)
                throw new ArgumentNullException(nameof(phoneNumber), "Phone number is required");
            if (address is null)
                throw new ArgumentNullException(nameof(address), "Address is required");

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                DateOfBirth = dateOfBirth,
                TaxId = taxId.Trim(),
                IdentificationId = identificationId.Trim(),
                Nationality = nationality.Trim(),
                _email = email,
                _phoneNumber = phoneNumber,
                _address = address,

                Status = CustomerStatus.PendingVerification,
                CreatedAt = DateTime.UtcNow,
            };

            return customer;

        }

        public void UpdateContactInformation(Email newEmail, PhoneNumber newPhoneNumber, Address newAddress)
        {
            if(Status == CustomerStatus.Closed)
                    throw new InvalidOperationException("Cannot update contact information for a closed customer.");

            _email = newEmail ?? throw new ArgumentNullException(nameof(newEmail));
            _phoneNumber = newPhoneNumber ?? throw new ArgumentNullException(nameof(newPhoneNumber));
            _address = newAddress ?? throw new ArgumentNullException(nameof(newAddress));
            UpdatedAt = DateTime.UtcNow;
        }

        public void CompleteKycVerification()
        {
            if (Status != CustomerStatus.PendingVerification)
                throw new InvalidOperationException($"Cannot complete KYC for customer in {Status} status");

            Status = CustomerStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Suspend(string reason)
        {
            if (Status == CustomerStatus.Closed)
                throw new InvalidOperationException("Cannot suspend a closed customer");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Suspension reason is required", nameof(reason));

            Status = CustomerStatus.Suspended;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reactivate()
        {
            if (Status != CustomerStatus.Suspended)
                throw new InvalidOperationException($"Cannot reactivate customer in {Status} status");

            Status = CustomerStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Close()
        {
            if (_accounts.Any(a => a.Status == AccountStatus.Active))
                throw new InvalidOperationException("Cannot close customer with active accounts. Close all accounts first.");

            Status = CustomerStatus.Closed;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool CanOpenNewAccount()
        {
            if (Status != CustomerStatus.Active)
                return false;

            if (_accounts.Count >= 10)
                return false;

            return true;
        }

        public string GetFullName() => $"{FirstName} {LastName}";

        public int GetAge()
        {
            var today = DateTime.UtcNow;
            var age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age))
                age--;
            return age;
        }
    }



    public enum  CustomerStatus
    {
        PendingVerification,
        Active,
        Suspended,
        Closed
    }

}
