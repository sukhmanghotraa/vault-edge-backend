using ErrorOr;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Repositories;
using VaultEdge.Application.Users.DTOs;
using VaultEdge.Domain.Common.Errors;

namespace VaultEdge.Application.Users.Queries.GetUserById
{
    public class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancelationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user is null) {
                return UserErrors.User.NotFound(request.UserId);
            }

            var foundUser = new UserDto
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
            };

            return foundUser;
        }
    }
}