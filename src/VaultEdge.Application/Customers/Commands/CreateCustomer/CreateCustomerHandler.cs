using ErrorOr;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Common.Errors;
using VaultEdge.Domain.Customer;
using VaultEdge.Domain.ValueObjects;

namespace VaultEdge.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerHandler : ICommandHandler<CreateCustomerCommand, Guid>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<ErrorOr<Guid>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _customerRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (existingUser != null)
            {
                return CustomerErrors.Customer.EmailAlreadyInUse;
            }

            var email = Email.Create(request.Email);
            var phoneNumber = PhoneNumber.Create(request.PhoneNumber);
            var address = Address.Create(request.Address);

            var newCustomer = Customer.Create(
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.TaxId,
                request.IdentificationId,
                request.Nationality,
                email,
                phoneNumber,
                address
            );

            await _customerRepository.AddAsync(newCustomer);
            await _customerRepository.SaveChangesAsync();
            return newCustomer.Id;
        }
    }
}
