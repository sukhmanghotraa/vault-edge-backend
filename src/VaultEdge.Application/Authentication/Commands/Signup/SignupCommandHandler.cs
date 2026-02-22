using ErrorOr;
using MediatR;
using VaultEdge.Application.Authentication.Common;
using VaultEdge.Application.Common.Interfaces.Authentication;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Common.Errors;
using VaultEdge.Domain.Customer;
using VaultEdge.Domain.ValueObjects;

namespace VaultEdge.Application.Authentication.Commands.Signup
{
    public class SignupCommandHandler: IRequestHandler<SignupCommand, ErrorOr<AuthenticationResult>>
    {

        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ICustomerRepository _userRepository;

        public SignupCommandHandler(IJwtTokenGenerator jwtTokenGenerator, ICustomerRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(SignupCommand command, CancellationToken cancellationToken)
        {
            var existingUserTask = _userRepository.GetByEmailAsync(command.Email, default);
            var existingUser = existingUserTask.GetAwaiter().GetResult();

            if(existingUser is not null)
            {
                return CustomerErrors.Customer.EmailAlreadyInUse;
            }

            var email = Email.Create(command.Email);
            var phoneNumber = PhoneNumber.Create(command.PhoneNumber);
            var address = Address.Create(command.Address);

            var newCustomer = Customer.Create(
                command.FirstName,
                command.LastName,
                command.DateOfBirth,
                command.TaxId,
                command.IdentificationId,
                command.Nationality,
                email,
                phoneNumber,
                address
            );

            await _userRepository.AddAsync(newCustomer);
            await _userRepository.SaveChangesAsync();

            var token = _jwtTokenGenerator.GenerateToken(newCustomer);

            return new AuthenticationResult(
                newCustomer,
                token);
        }
    }
}