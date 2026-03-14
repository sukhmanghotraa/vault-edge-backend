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

            public static Error AlreadyExist  => Error.Conflict(
                code: "Account.AlreadyExists",
                description: "An account with the same details already exists.");

            public static Error DuplicateAccountNumber => Error.Conflict(
                code: "Account.DuplicateAccountNumber",
                description: "An account with the same account number already exists.");

            public static Error CustomerCannotOpenAccount => Error.Conflict(
                code: "Account.CustomerCannotOpenAccount",
                description: "The customer is not eligible to open a new account.");

            public static Error CurrencyMismatch => Error.Conflict(
                code: "Account.CurrencyMismatch",
                description: "The currency of the account does not match the expected currency.");

            public static Error InvalidCurrency => Error.Conflict(
                code: "Account.InvalidCurrency",
                description: "The provided currency code is invalid.");

            public static Error InvalidLimit => Error.Conflict(
                code: "Account.InvalidLimit",
                description: "The provided daily withdrawal limit is invalid.");

            public static Error LimitCurrencyMismatch => Error.Conflict(
                code: "Account.InvalidLimit",
                description: "The currency of the daily withdrawal limit does not match the account currency.");

            public static Error MonthlyLimitLessThanDailyLimit => Error.Conflict(
                code: "Account.MonthlyLimitLessThanDailyLimit",
                description: "The monthly withdrawal limit must be greater than or equal to the daily withdrawal limit.");
        }
    }
}