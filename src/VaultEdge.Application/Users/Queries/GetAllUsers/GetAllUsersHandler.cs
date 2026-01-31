using MediatR;
using VaultEdge.Application.Repositories;
using VaultEdge.Application.Users.DTOs;
using VaultEdge.Domain.Errors;
using VaultEdge.Domain.Shared;

namespace VaultEdge.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserDto>>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken = default)
        {
            var users = await _userRepository.GetAllAsync();
            if (users == null)
            {
                return Result.Failure<IEnumerable<UserDto>>(
                    DomainErrors.User.NoneFound);
            }

            var usersCopy =  users.Select(user => new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                TaxId = user.TaxId,
                IdentificationId = user.IdentificationId,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            });

            return Result.Success<IEnumerable<UserDto>>(usersCopy);
        }
    }
}
