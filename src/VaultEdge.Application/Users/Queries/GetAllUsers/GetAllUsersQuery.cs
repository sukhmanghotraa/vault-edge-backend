
using MediatR;
using VaultEdge.Domain.Entities;
using VaultEdge.Domain.Repositories;

namespace VaultEdge.Application.Users.Queries.GetUserById
{
    public class GetAllUsersQuery : IRequest<IEnumerable<User>> { }
}
