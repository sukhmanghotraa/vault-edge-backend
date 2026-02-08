using FluentValidation;

namespace VaultEdge.Application.Authentication.Commands.Signup
{
    public class SignupCommandValidator : AbstractValidator<SignupCommand>
    {
        public SignupCommandValidator()
        {
            RuleFor(x => x.firstName).NotEmpty();
            RuleFor(x => x.lastName).NotEmpty();
            RuleFor(x => x.email).NotEmpty().EmailAddress();
            RuleFor(x => x.passwordHash).NotEmpty();
            RuleFor(x => x.dateOfBirth).NotEmpty().LessThan(DateTime.Now);
            RuleFor(x => x.taxId).NotEmpty();
            RuleFor(x => x.identificationId).NotEmpty();
            RuleFor(x => x.nationality).NotEmpty();
            RuleFor(x => x.phoneNumber).NotEmpty().Matches(@"^\+?[1-9]\d{1,14}$");
            RuleFor(x => x.address).NotEmpty();
        }
    }
}
