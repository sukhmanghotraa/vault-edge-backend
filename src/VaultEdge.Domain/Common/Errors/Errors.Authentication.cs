using ErrorOr;

namespace VaultEdge.Domain.Common.Errors
{
    public static class AuthenticationErrors
    {
        public static class Authentication
        {             
            public static Error InvalidCredentials => Error.Validation(
                code: "Auth.InvalidCredentials",
                description: "The provided credentials are invalid");

            public static Error IndentityCreationFailed => Error.Validation(
                code: "Auth.IdentityCreationFailed",
                description: "Failed to create user identity");
        }
    }
}