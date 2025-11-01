
using MediatR;
using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<User?>
    {
        public Guid Id { get; set; }

        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
