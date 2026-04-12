using ErrorOr;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Accounts.DTOs;

namespace VaultEdge.Application.Accounts.Queries.GetAccountsByCustomerId
{
    public record GetAccountsByCustomerIdQuery(Guid CustomerId) : IQuery<List<AccountDto>>;
}