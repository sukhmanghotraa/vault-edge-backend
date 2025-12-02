
using MediatR;
using VaultEdge.Application.Users.DTOs;
using VaultEdge.Domain.Shared;

namespace VaultEdge.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<Result<IEnumerable<UserDto>>> { }
}

