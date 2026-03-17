using ErrorOr;
using MediatR;
using VaultEdge.Application.Authentication.Common;
using VaultEdge.Application.Common.Interfaces.Authentication;
using VaultEdge.Application.Common.Interfaces.Persistence;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Common.Errors;
using VaultEdge.Domain.Customer;
using VaultEdge.Domain.ValueObjects;

namespace VaultEdge.Application.Authentication.Commands.Signup
{
    public class SignupCommandHandler: IRequestHandler<SignupCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ICustomerRepository _customerRepository;

        public SignupCommandHandler(IIdentityService identityService,IJwtTokenGenerator jwtTokenGenerator, ICustomerRepository userRepository)
        {
            _identityService = identityService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _customerRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(SignupCommand command, CancellationToken cancellationToken)
        {
            var existingCustomer = await _customerRepository.GetByEmailAsync(command.Email, cancellationToken);

            if(existingCustomer is not null)
            {
                return CustomerErrors.Customer.EmailAlreadyInUse;
            }

            var email = Email.Create(command.Email);
            if (email is null) 
            {
                return AuthenticationErrors.Authentication.InvalidCredentials;
            }

            var phoneNumber = PhoneNumber.Create(command.PhoneNumber);
            if (phoneNumber is null)
            {
                return AuthenticationErrors.Authentication.InvalidCredentials;
            }

            var address = Address.Create(command.Address);
            if (address is null)
            {
                return AuthenticationErrors.Authentication.InvalidCredentials;
            }
            
            var customer = Customer.Create(
                firstName: command.FirstName,
                lastName: command.LastName,
                dateOfBirth: command.DateOfBirth,
                taxId: command.TaxId,
                identificationId: command.IdentificationId,
                nationality: command.Nationality,
                email: email,
                phoneNumber: phoneNumber,
                address: address
            );

            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();

            var identityResult = await _identityService.CreateUserAsync(
                customerId: customer.Id, 
                email: command.Email, 
                password: command.Password);

            if (!identityResult.Succeeded)
            {
                return AuthenticationErrors.Authentication.IndentityCreationFailed;
            }

            var securityStamp = await _identityService.GetSecurityStampAsync(customer.Id);

            var accessToken = _jwtTokenGenerator.GenerateAccesssToken(customer, securityStamp);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            return new AuthenticationResult(
                customer,
                accessToken,
                refreshToken);
        }
    }
}