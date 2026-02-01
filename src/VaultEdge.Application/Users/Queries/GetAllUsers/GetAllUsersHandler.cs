using ErrorOr;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Repositories;
using VaultEdge.Application.Users.DTOs;
using VaultEdge.Domain.Common.Errors;

namespace VaultEdge.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersHandler : IQueryHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<List<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken = default)
        {
            var users = await _userRepository.GetAllAsync();
            if (users == null)
            {
                return UserErrors.User.NoneFound;
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

            return usersCopy.ToList();
        }
    }
}