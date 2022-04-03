namespace VaultManager.Models;

public record IdentityEntry(Guid Identifier, string FirstName, string LastName, string Email, string PhoneNumber, string Address, string Title, string City, string ZipCode, List<Guid> Tags) : BaseTagEntry(Identifier, Tags);
