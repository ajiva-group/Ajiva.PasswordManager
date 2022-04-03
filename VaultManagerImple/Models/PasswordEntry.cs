using KeyValuePair = VaultManager.Models.KeyValuePair;

namespace VaultManager.Models;

public record PasswordEntry(Guid Identifier, Uri Domain, string User, string Password, Guid? TowFactorEntry, List<Guid> Tags, List<KeyValuePair> AdditionalInfo)  : BaseTagEntry(Identifier, Tags);
