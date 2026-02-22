using ErrorOr;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Common.Errors;

namespace VaultEdge.Application.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerHandler: ICommandHandler<DeleteCustomerCommand, Guid>
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<ErrorOr<Guid>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if(customer == null)
            {
                return CustomerErrors.Customer.NotFound(request.CustomerId);
            }

            await _customerRepository.DeleteCustomerAsync(customer.Id);
            await _customerRepository.SaveChangesAsync();
            return customer.Id;
        }
    }
}