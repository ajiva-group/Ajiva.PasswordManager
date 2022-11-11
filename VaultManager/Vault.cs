using System.ComponentModel.DataAnnotations.Schema;

namespace VaultManager;

public class Vault
{
    public event Action VaultChanged;
    public event Action<PasswordEntry, PasswordEntry> PasswordEntryChanged;

    public string Name { get; }

    public Vault(string name)
    {
        Name = name;
    }

    public Dictionary<Guid, PasswordEntry> Passwords { get; set; } = new();
    public Dictionary<Guid, TwoFactorEntry> TwoFactors { get; set; } = new();
    public Dictionary<Guid, IdentityEntry> Identities { get; set; } = new();
    public Dictionary<Guid, WebSideEntry> WebSides { get; set; } = new();
    public Dictionary<Guid, FileNoteEntry> Files { get; set; } = new();
    public Dictionary<Guid, TagEntry> Tags { get; set; } = new();

    public PasswordEntry Update(PasswordEntry passwordEntry)
    {
        var old = Passwords[passwordEntry.Id];
        //copy vales from old to new if null
        if (passwordEntry.Username == null)
            passwordEntry.Username = old.Username;
        if (passwordEntry.Password == null)
            passwordEntry.Password = old.Password;
        if (passwordEntry.WebSide == null)
            passwordEntry.WebSide = old.WebSide;
        if (passwordEntry.Notes == null)
            passwordEntry.Notes = old.Notes;

        if (passwordEntry.WebSide.Description == null)
            passwordEntry.WebSide.Description = old.WebSide.Description;
        if (passwordEntry.WebSide.Domain == null)
            passwordEntry.WebSide.Domain = old.WebSide.Domain;

        Passwords[passwordEntry.Id] = passwordEntry;
        OnPasswordEntryChanged(passwordEntry, old);
        return passwordEntry;
    }

    protected virtual void OnPasswordEntryChanged(PasswordEntry newEntry, PasswordEntry oldEntry)
    {
        PasswordEntryChanged?.Invoke(newEntry, oldEntry);
        OnVaultChanged();
    }

    protected virtual void OnVaultChanged()
    {
        VaultChanged?.Invoke();
    }
}
public class PasswordEntry
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    public WebSideEntry WebSide { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Notes { get; set; }

    public Guid? TwoFactor { get; set; }
    public List<TagEntry>? Tags { get; set; }
}
public class TwoFactorEntry
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    public string Type { get; set; }
    public string Secret { get; set; }

    public Guid Credential { get; set; }

    public List<TagEntry>? Tags { get; set; }
}
public class IdentityEntry
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }

    public string Notes { get; set; }
    public List<TagEntry>? Tags { get; set; }
}
public class WebSideEntry
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    public Uri Domain { get; set; }
    public List<PasswordEntry> Passwords { get; set; }
    public WebSideSettings? Settings { get; set; }

    public List<TagEntry>? Tags { get; set; }
}
public class WebSideSettings
{
}
public class FileNoteEntry
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string Notes { get; set; }
    public byte[] FileData { get; set; }

    public List<TagEntry>? Tags { get; set; }
}
public class TagEntry
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public List<Guid> Passwords { get; set; }
    public List<Guid> TwoFactors { get; set; }
    public List<Guid> Identities { get; set; }
    public List<Guid> WebSides { get; set; }
    public List<Guid> Files { get; set; }
}
