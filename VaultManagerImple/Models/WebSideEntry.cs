namespace VaultManager.Models;

public record WebSideEntry(Guid Identifier, Uri Uri, List<Guid> PasswordEntries, WebsideSettings WebSideSettings, List<Guid> Tags) : BaseTagEntry(Identifier, Tags);
