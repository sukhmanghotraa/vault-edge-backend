using ErrorOr;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Repositories;
using VaultEdge.Application.Customers.DTOs;
using VaultEdge.Domain.Common.Errors;

namespace VaultEdge.Application.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdHandler : IQueryHandler<GetCustomerByIdQuery, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByIdHandler(ICustomerRepository userRepository)
        {
            _customerRepository = userRepository;
        }

        public async Task<ErrorOr<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancelationToken = default)
        {
            var user = await _customerRepository.GetByIdAsync(request.CustomerId);

            if (user is null) {
                return CustomerErrors.Customer.NotFound(request.CustomerId);
            }

            var foundUser = new CustomerDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                TaxId = user.TaxId,
                IdentificationId = user.IdentificationId,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return foundUser;
        }
    }
}