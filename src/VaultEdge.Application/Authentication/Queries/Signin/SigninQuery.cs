using ErrorOr;
using MediatR;
using VaultEdge.Application.Authentication.Common;

namespace VaultEdge.Application.Authentication.Queries.Signin
{
    public record SigninQuery(string Email, string Password): IRequest<ErrorOr<AuthenticationResult>>;
}