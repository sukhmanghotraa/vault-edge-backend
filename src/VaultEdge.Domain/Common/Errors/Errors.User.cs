using ErrorOr;

namespace VaultEdge.Domain.Common.Errors
{
    public static class UserErrors
    {
        public static class User
        {
            public static  Error EmailAlreadyInUse => Error.Conflict(
                code: "User.EmailAlreadyInUse",
                description: "The provided email is already in use.");

            public static  Error NoneFound => Error.Conflict(
                code: "Users.NotFound",
                description: $"No users were not found.");

            public static  Func<Guid, Error> NotFound = id =>  Error.Conflict(
                code: "User.NotFound",
                description: $"The member with the identifier {id} was not found.");

            public static Error InvalidCredentials => Error.Validation(
                code: "User.InvalidCredentials",
                description: "The provided credentials are invalid");
        }   
    }
}