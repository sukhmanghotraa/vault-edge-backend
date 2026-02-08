using FluentValidation;

namespace VaultEdge.Application.Authentication.Queries.Signin
{
    public class SigninQueryValidator: AbstractValidator<SigninQuery>
    {
        public SigninQueryValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
