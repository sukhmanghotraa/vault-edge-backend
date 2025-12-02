using MediatR;
using VaultEdge.Application.Accounts.DTOs;
using VaultEdge.Domain.Shared;

namespace VaultEdge.Application.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdQuery : IRequest<Result<AccountDto?>>
    {
        public Guid Id { get; set; }

        public GetAccountByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
