namespace VaultManager.Models;

public record FileEntry(Guid Identifier, byte[] Content, List<Guid> Tags) : BaseTagEntry(Identifier, Tags);
