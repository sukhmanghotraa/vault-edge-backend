using System.ComponentModel.DataAnnotations;

namespace VaultEdge.Domain.User
{
    public class User
    {
        /// <summary>
        ///  User information
        /// </summary>
        public Guid Id { get; private set; }
        public Guid CustomerId { get; set; }

        public  string FirstName { get; set; } = string.Empty;
        public  string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; private set; }

        /// <summary>
        /// A government-issued id (e.g. National ID, Tax Number etc.)
        /// </summary>
        public string TaxId { get; private set; } = string.Empty;
        public string IdentificationId { get; private set; } = string.Empty;
        public  string Nationality { get; private set; } = string.Empty;

        /// <summary>
        /// Users login credentials.... a salted and hashed representation of the user's password
        /// </summary>
        public  string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;

        /// <summary>
        /// a random value that changes when user's credentials change...
        /// to invalidate old login sessions and tokens
        /// </summary>
        public string SecurityStamp { get; set; } = string.Empty;
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// Contact information
        /// </summary>
        public  string PhoneNumber { get; set; } = string.Empty;
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Physical addrss
        /// </summary>
        public  string Address { get; set; } = string.Empty;

        /// <summary>
        /// lockout information
        /// </summary>
        public DateTime? LockoutEnd { get; set; }

        /// <summary>
        /// Tracks number of consecutive failed login attempts
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// Status information
        /// </summary>
        public  UserStatus Status{ get; set; }

        /// <summary>
        /// Role information for authorization
        /// </summary>
        public  UserRole Role { get; set; }

        /// <summary>
        /// Timestamps
        /// </summary>
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// Bank accounts associated with the user
        /// </summary>
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

        /// <summary>
        /// private constructor for EF Core
        /// </summary>
        private User() { }

        /// <summary>
        /// Constructor to create a new user
        /// </summary>
        public User(string firstName, string lastName, string passwordHash, DateTime dateOfBirth, string taxId, string identificationId, string nationality, string email, string phoneNumber, string address)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required");
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Email is requred");
            if (string.IsNullOrWhiteSpace(taxId)) throw new ArgumentException("Email is requred");
            if (string.IsNullOrWhiteSpace(identificationId)) throw new ArgumentException("Email is requred");
            if (string.IsNullOrWhiteSpace(nationality)) throw new ArgumentException("Email is requred");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is requred");
            if (string.IsNullOrWhiteSpace(phoneNumber)) throw new ArgumentException("Email is requred");
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Email is requred");

            Id = Guid.NewGuid();
            CustomerId = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            PasswordHash = passwordHash;
            DateOfBirth = dateOfBirth;
            TaxId = taxId;
            IdentificationId = identificationId;
            Nationality = nationality;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;

            Status = UserStatus.PedningVerification;
            Role = UserRole.Customer;
            SecurityStamp = Guid.NewGuid().ToString("N");
            CreatedAt = DateTime.UtcNow;
            Accounts = new List<Account>();
        }
    }


    public enum  UserStatus
    {
        PedningVerification,
        Active,
        Suspended,
        Closed
    }

    public enum UserRole
    {
        Customer,
        Admin,
        Support
    }
}
