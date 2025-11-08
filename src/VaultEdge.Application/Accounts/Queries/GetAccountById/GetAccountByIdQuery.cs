using MediatR;
using VaultEdge.Application.Accounts.DTOs;

namespace VaultEdge.Application.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdQuery : IRequest<AccountDto?>
    {
        public Guid Id { get; set; }

        public GetAccountByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
