using ErrorOr;

namespace VaultEdge.Domain.Common.Errors
{
    public static class AccountErrors
    {
        public static class Account
        {
            public static Error NoneFound => Error.Conflict(
                code: "Accounts.NotFound",
                description: $"No accounts were not found.");

            public static Func<Guid, Error> NotFound = id => Error.Conflict(
                code: "Account.NotFound",
                description: $"The account with the identifier {id} was not found.");

            public static Error InvalidCredentials => Error.Validation(
                code: "Account.InvalidCredentials",
                description: "The provided credentials are invalid");
        }
    }
}