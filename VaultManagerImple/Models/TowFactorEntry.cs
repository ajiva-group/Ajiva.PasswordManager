namespace VaultManager.Models;

public record TowFactorEntry(Guid Identifier, string TowFactorSecret, PasswordEntry? PasswordEntry, List<Guid> Tags) : BaseTagEntry(Identifier, Tags);
