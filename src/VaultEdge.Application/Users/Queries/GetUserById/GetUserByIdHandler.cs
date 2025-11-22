
using MediatR;
using VaultEdge.Application.Users.DTOs;
using VaultEdge.Domain.Errors;
using VaultEdge.Domain.Repositories;
using VaultEdge.Domain.Shared;

namespace VaultEdge.Application.Users.Queries.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto?>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserDto?>> Handle(GetUserByIdQuery request, CancellationToken cancelationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null) {
                return Result.Failure<UserDto?>(
                    DomainErrors.User.NotFound(request.Id));
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

            return Result.Success(foundUser);
        }
    }
}
