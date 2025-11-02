using MediatR;
using VaultEdge.Application.Users.DTOs;
using VaultEdge.Domain.Repositories;

namespace VaultEdge.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken = default)
        {
            var users = await _userRepository.GetAllAsync();
            if(users == null)
            {
                return Enumerable.Empty<UserDto>();
            }

            return users.Select(user => new UserDto
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
        }
    }
}
