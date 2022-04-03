using VaultManager.Models;

namespace VaultManager.Vaults;

public class IdentityVault : ISerializableVault
{
    private List<IdentityEntry> _identityEntries;

    public IdentityVault()
    {
        _identityEntries = new List<IdentityEntry>();
    }

    public void AddIdentity(IdentityEntry identityEntry)
    {
        _identityEntries.Add(identityEntry);
    }

    /// <inheritdoc />
    public IEnumerable<BaseEntry> ToSerializable()
    {
        return _identityEntries;
    }

    /// <inheritdoc />
    public void FromSerializable(IEnumerable<BaseEntry> serializable)
    {
        _identityEntries = serializable.OfType<IdentityEntry>().ToList();
    }
}
