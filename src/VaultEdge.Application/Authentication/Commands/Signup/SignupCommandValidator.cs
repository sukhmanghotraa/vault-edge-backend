using FluentValidation;

namespace VaultEdge.Application.Authentication.Commands.Signup
{
    public class SignupCommandValidator : AbstractValidator<SignupCommand>
    {
        public SignupCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.DateOfBirth).NotEmpty().LessThan(DateTime.Now);
            RuleFor(x => x.TaxId).NotEmpty();
            RuleFor(x => x.IdentificationId).NotEmpty();
            RuleFor(x => x.Nationality).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+?[1-9]\d{1,14}$");
            RuleFor(x => x.Address).NotEmpty();
        }
    }
}
