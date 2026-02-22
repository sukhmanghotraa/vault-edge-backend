
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Customers.DTOs;

namespace VaultEdge.Application.Customers.Queries.GetCustomerById
{
    public record GetCustomerByIdQuery(Guid CustomerId) : IQuery<CustomerDto>;
}