
using MediatR;
using VaultEdge.Application.Users.DTOs;

namespace VaultEdge.Application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserDto?>
    {
        public Guid Id { get; set; }

        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
