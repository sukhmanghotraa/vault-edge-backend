using ErrorOr;

namespace VaultEdge.Domain.Common.Errors
{
    public static class CustomerErrors
    {
        public static class Customer
        {
            public static  Error EmailAlreadyInUse => Error.Conflict(
                code: "Customer.EmailAlreadyInUse",
                description: "The provided email is already in use.");

            public static  Error NoneFound => Error.Conflict(
                code: "Customers.NotFound",
                description: $"No customers were not found.");

            public static  Func<Guid, Error> NotFound = id =>  Error.Conflict(
                code: "Customer.NotFound",
                description: $"The member with the identifier {id} was not found.");

            public static Error InvalidCredentials => Error.Validation(
                code: "Customer.InvalidCredentials",
                description: "The provided credentials are invalid");
        }   
    }
}