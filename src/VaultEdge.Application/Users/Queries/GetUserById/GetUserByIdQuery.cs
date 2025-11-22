
using MediatR;
using VaultEdge.Application.Users.DTOs;
using VaultEdge.Domain.Shared;

namespace VaultEdge.Application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<Result<UserDto?>>
    {
        public Guid Id { get; set; }

        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
