using VaultManager.Models;

namespace VaultManager.Vaults;

public class PasswordVault : ISerializableVault
{
    private List<PasswordEntry> _passwords;

    public List<PasswordEntry> Passwords
    {
        get => _passwords;
        set => _passwords = value ?? throw new ArgumentNullException(nameof(value));
    }

    public PasswordVault()
    {
        _passwords = new List<PasswordEntry>();
    }

    public void AddPassword(PasswordEntry password)
    {
        _passwords.Add(password);
    }

    public IEnumerable<BaseEntry> ToSerializable()
    {
        return _passwords;
    }

    /// <inheritdoc />
    public void FromSerializable(IEnumerable<BaseEntry> serializable)
    {
        _passwords = serializable.OfType<PasswordEntry>().ToList();
    }
}
