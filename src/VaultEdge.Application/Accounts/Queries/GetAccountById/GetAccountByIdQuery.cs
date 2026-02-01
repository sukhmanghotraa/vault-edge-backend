using ErrorOr;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Accounts.DTOs;

namespace VaultEdge.Application.Accounts.Queries.GetAccountById
{
    public record GetAccountByIdQuery(Guid AccountId) : IQuery<AccountDto>;
}