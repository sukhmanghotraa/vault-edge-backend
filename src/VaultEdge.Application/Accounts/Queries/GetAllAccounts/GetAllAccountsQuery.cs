using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Accounts.DTOs;

namespace VaultEdge.Application.Accounts.Queries.GetAllAccounts;

public record GetAllAccountsQuery : IQuery<List<AccountDto>>;