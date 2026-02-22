using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaultEdge.Application.Customers.Commands.CreateCustomer;
using VaultEdge.Application.Customers.Commands.DeleteCustomer;
using VaultEdge.Application.Customers.Queries.GetAllCustomers;
using VaultEdge.Application.Customers.Queries.GetCustomerById;

namespace VaultEdge.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ApiController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateCustomerCommand command)
        {
            var result = await _mediator.Send(command); 
        
            return result.Match(
                userId => CreatedAtAction(nameof(GetUserById), new { userId = userId }, userId),
                _ => Problem(statusCode: StatusCodes.Status409Conflict, title: "User already exit.")
            );
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var authenticatedUserId = User.FindFirst("sub")?.Value;

            // This only exists if authorization succeeded
            Console.WriteLine($"Authorized user: {authenticatedUserId}");

            var query = new GetCustomerByIdQuery(userId);
            var result = await _mediator.Send(query);

            return result.Match(
                user => Ok(user),
                _ => Problem(statusCode: StatusCodes.Status404NotFound, title: "User not found.")
            );
        }

        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> DeleteUserById(Guid userId)
        {
            var query = new DeleteCustomerCommand(userId);
            var result = await _mediator.Send(query);

            return result.Match(
                deletedUserId => Ok(deletedUserId),
                _ => Problem(statusCode: StatusCodes.Status404NotFound, title: "User not found.")
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var query = new GetAllCustomersQuery();
            var users = await _mediator.Send(query);

            return users.Match(
                userList => Ok(userList),
                _ => Problem(statusCode: StatusCodes.Status404NotFound, title: "No users found.")
            );
        }
    }
}