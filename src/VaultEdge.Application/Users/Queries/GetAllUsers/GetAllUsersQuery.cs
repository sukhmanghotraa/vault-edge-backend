
using MediatR;
using VaultEdge.Application.Users.DTOs;

namespace VaultEdge.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>> { }
}

