using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaultEdge.Application.Customers.Commands.DeleteCustomer;
using VaultEdge.Application.Customers.Queries.GetAllCustomers;
using VaultEdge.Application.Customers.Queries.GetCustomerById;

namespace VaultEdge.Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : ApiController
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{customerId:guid}")]
        public async Task<IActionResult> GetCustomerById(Guid customerId)
        {
            var query = new GetCustomerByIdQuery(customerId);
            var result = await _mediator.Send(query);

            return result.Match(
                customer => Ok(customer),
                _ => Problem(statusCode: StatusCodes.Status404NotFound, title: "Customer not found.")
            );
        }

        [HttpDelete("{customerId:guid}")]
        public async Task<IActionResult> DeleteCustomerById(Guid customerId)
        {
            var query = new DeleteCustomerCommand(customerId);
            var result = await _mediator.Send(query);

            return result.Match(
                deletedCustomerId => Ok(deletedCustomerId),
                _ => Problem(statusCode: StatusCodes.Status404NotFound, title: "Customer not found.")
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var query = new GetAllCustomersQuery();
            var customers = await _mediator.Send(query);

            return customers.Match(
                customerList => Ok(customerList),
                _ => Problem(statusCode: StatusCodes.Status404NotFound, title: "No customers found.")
            );
        }
    }
}