namespace VaultEdge.Api.Authentication
{
    public record SignupRequest(
         string FirstName,
         string LastName,
         string PasswordHash,
         DateTime DateOfBirth,
         string TaxId,
         string IdentificationId,
         string Nationality,
         string Email,
         string PhoneNumber,
         string Address );
}
