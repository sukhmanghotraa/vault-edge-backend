using MediatR;
using VaultEdge.Application.Accounts.DTOs;

namespace VaultEdge.Application.Accounts.Queries.GetAllAccounts;

public class GetAllAccountsQuery : IRequest<IEnumerable<AccountDto>> { }
