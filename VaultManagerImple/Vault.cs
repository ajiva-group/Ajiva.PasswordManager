using VaultManager.Models;
using VaultManager.Serializable;
using VaultManager.Vaults;

namespace VaultManager;

public class Vault : ISerializable<SerializableVault>
{
    public PasswordVault PasswordVault { get; set; }
    public FileVault FileVault { get; set; }
    public TowFactorVault TowFactorVault { get; set; }
    public IdentityVault IdentityVault { get; set; }

    public Vault()
    {
        PasswordVault = new PasswordVault();
        FileVault = new FileVault();
        TowFactorVault = new TowFactorVault();
        IdentityVault = new IdentityVault();
    }

    public Vault(SerializableVault serializableVault) : this()
    {
        FromSerializable(serializableVault);
    }

    public SerializableVault ToSerializable()
    {
        var serializableVault = new SerializableVault();
        serializableVault.Add(PasswordVault.ToSerializable());
        serializableVault.Add(FileVault.ToSerializable());
        serializableVault.Add(TowFactorVault.ToSerializable());
        serializableVault.Add(IdentityVault.ToSerializable());
        return serializableVault;
    }

    /// <inheritdoc />
    public void FromSerializable(SerializableVault serializable)
    {
        PasswordVault.FromSerializable(serializable.Of<PasswordEntry>());
        FileVault.FromSerializable(serializable.Of<FileEntry>());
        TowFactorVault.FromSerializable(serializable.Of<TowFactorEntry>());
        IdentityVault.FromSerializable(serializable.Of<IdentityEntry>());
    }
}
