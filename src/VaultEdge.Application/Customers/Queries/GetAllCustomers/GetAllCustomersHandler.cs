using ErrorOr;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Repositories;
using VaultEdge.Application.Customers.DTOs;
using VaultEdge.Domain.Common.Errors;

namespace VaultEdge.Application.Customers.Queries.GetAllCustomers
{
    public class GetAllCustomersHandler : IQueryHandler<GetAllCustomersQuery, List<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetAllCustomersHandler(ICustomerRepository userRepository)
        {
            _customerRepository = userRepository;
        }

        public async Task<ErrorOr<List<CustomerDto>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken = default)
        {
            var users = await _customerRepository.GetAllAsync();

            if (users is null)
            {
                return CustomerErrors.Customer.NoneFound;
            }

            var usersCopy =  users.Select(user => new CustomerDto
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
            });

            return usersCopy.ToList();
        }
    }
}