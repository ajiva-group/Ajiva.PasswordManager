namespace VaultManager.Models;

public record BaseEntry(Guid Identifier);
public record BaseTagEntry(Guid Identifier, List<Guid> Tags) : BaseEntry(Identifier);
