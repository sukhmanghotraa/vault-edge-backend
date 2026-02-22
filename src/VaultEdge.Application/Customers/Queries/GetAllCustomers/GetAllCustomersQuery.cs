using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Customers.DTOs;

namespace VaultEdge.Application.Customers.Queries.GetAllCustomers;
public record GetAllCustomersQuery : IQuery<List<CustomerDto>>;