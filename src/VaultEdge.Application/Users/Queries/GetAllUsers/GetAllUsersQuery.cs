
using MediatR;
using VaultEdge.Domain.Entities;
using VaultEdge.Domain.Repositories;

namespace VaultEdge.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<IEnumerable<User>> { }
}

