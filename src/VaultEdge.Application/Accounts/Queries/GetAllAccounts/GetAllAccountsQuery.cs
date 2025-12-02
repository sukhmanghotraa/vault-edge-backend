using MediatR;
using VaultEdge.Application.Accounts.DTOs;
using VaultEdge.Domain.Shared;

namespace VaultEdge.Application.Accounts.Queries.GetAllAccounts;

public class GetAllAccountsQuery : IRequest<Result<IEnumerable<AccountDto>>> { }
