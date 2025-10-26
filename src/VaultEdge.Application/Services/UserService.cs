using VaultEdge.Application.Interfaces;
using VaultEdge.Domain.Entities;

namespace VaultEdge.Application.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUserAsync(CreateUserRequest request)
        {
            var user = new User(
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.TaxId,
                request.IdentificationId,
                request.Nationality,
                request.Email,
                request.PhoneNumber,
                request.Address
                );

            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }
    }
}
