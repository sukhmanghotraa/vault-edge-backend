
using MediatR;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Users.DTOs;

namespace VaultEdge.Application.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid UserId) : IQuery<UserDto>;
}