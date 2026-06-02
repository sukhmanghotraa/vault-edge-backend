using VaultEdge.Application.Authentication.Commands.Signup;
using VaultEdge.Application.Common.Interfaces.Authentication;
using VaultEdge.Application.Common.Interfaces.Persistence;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Customer;

namespace VaultEdge.UnitTests.Authentication;

public class SignupCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenIdentityCreationFails_DoesNotCommitCustomerSeparately()
    {
        var identityService = new FakeIdentityService(IdentityResult.Failure(["Password is too weak"]));
        var jwtTokenGenerator = new FakeJwtTokenGenerator();
        var customerRepository = new FakeCustomerRepository();
        var handler = new SignupCommandHandler(identityService, jwtTokenGenerator, customerRepository);

        var result = await handler.Handle(CreateValidCommand(), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Single(customerRepository.AddedCustomers);
        Assert.Equal(0, customerRepository.SaveChangesCalls);
        Assert.Equal(customerRepository.AddedCustomers.Single().Id, identityService.CreatedCustomerId);
    }

    [Fact]
    public async Task Handle_WhenIdentityCreationSucceeds_DoesNotCallRepositorySaveChanges()
    {
        var identityService = new FakeIdentityService(IdentityResult.Success());
        var jwtTokenGenerator = new FakeJwtTokenGenerator();
        var customerRepository = new FakeCustomerRepository();
        var handler = new SignupCommandHandler(identityService, jwtTokenGenerator, customerRepository);

        var result = await handler.Handle(CreateValidCommand(), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Single(customerRepository.AddedCustomers);
        Assert.Equal(0, customerRepository.SaveChangesCalls);
    }

    private static SignupCommand CreateValidCommand() =>
        new(
            FirstName: "Ada",
            LastName: "Lovelace",
            Password: "Password123!",
            DateOfBirth: new DateTime(1990, 1, 1),
            TaxId: "TAX123456",
            IdentificationId: "ID123456",
            Nationality: "Italian",
            Email: "ada@example.com",
            PhoneNumber: "393331234567",
            Address: "1 Test Street");

    private sealed class FakeCustomerRepository : ICustomerRepository
    {
        public List<Customer> AddedCustomers { get; } = [];
        public int SaveChangesCalls { get; private set; }

        public Task AddAsync(Customer customer)
        {
            AddedCustomers.Add(customer);
            return Task.CompletedTask;
        }

        public Task<Customer?> GetByIdAsync(Guid id) =>
            Task.FromResult(AddedCustomers.FirstOrDefault(customer => customer.Id == id));

        public Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
            Task.FromResult(AddedCustomers.FirstOrDefault(customer => customer.Email == email));

        public Task<Customer?> GetByCustomerIdAsync(Guid customerId) =>
            Task.FromResult(AddedCustomers.FirstOrDefault(customer => customer.Id == customerId));

        public Task<IEnumerable<Customer>> GetAllAsync() =>
            Task.FromResult<IEnumerable<Customer>>(AddedCustomers);

        public Task<Customer> CreateCustomerAsync(Customer customer)
        {
            AddedCustomers.Add(customer);
            SaveChangesCalls++;
            return Task.FromResult(customer);
        }

        public Task SaveChangesAsync()
        {
            SaveChangesCalls++;
            return Task.CompletedTask;
        }

        public Task<Guid> DeleteCustomerAsync(Guid id) =>
            Task.FromResult(id);
    }

    private sealed class FakeIdentityService : IIdentityService
    {
        private readonly IdentityResult _createUserResult;

        public FakeIdentityService(IdentityResult createUserResult)
        {
            _createUserResult = createUserResult;
        }

        public Guid CreatedCustomerId { get; private set; }

        public Task<IdentityVerificationResult> AuthenticateAsync(string email, string password) =>
            Task.FromResult(IdentityVerificationResult.Success(Guid.NewGuid(), email, "security-stamp"));

        public Task<IdentityResult> CreateUserAsync(Guid customerId, string email, string password)
        {
            CreatedCustomerId = customerId;
            return Task.FromResult(_createUserResult);
        }

        public Task<IdentityResult> ChangePasswordAsync(Guid customerId, string currentPassword, string newPassword) =>
            Task.FromResult(IdentityResult.Success());

        public Task<IdentityResult> ConfirmEmailAsync(Guid customerId, string token) =>
            Task.FromResult(IdentityResult.Success());

        public Task<bool> IsLockedOutAsync(Guid customerId) =>
            Task.FromResult(false);

        public Task<string> GetSecurityStampAsync(Guid customerId) =>
            Task.FromResult("security-stamp");
    }

    private sealed class FakeJwtTokenGenerator : IJwtTokenGenerator
    {
        public string GenerateAccesssToken(Customer user, string? securityStamp = null) => "access-token";

        public string GenerateRefreshToken() => "refresh-token";
    }
}
