using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Users.DTOs;

namespace VaultEdge.Application.Users.Queries.GetAllUsers;
public record GetAllUsersQuery : IQuery<List<UserDto>>;