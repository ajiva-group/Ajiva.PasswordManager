using VaultManager.Models;

namespace VaultManager.Vaults;

public class TowFactorVault:ISerializableVault
{
    private List<TowFactorEntry> _towFactorEntries;

    public TowFactorVault()
    {
        _towFactorEntries = new List<TowFactorEntry>();
    }

    public void AddTowFactor(TowFactorEntry towFactorEntry)
    {
        _towFactorEntries.Add(towFactorEntry);
    }

    /// <inheritdoc />
    public IEnumerable<BaseEntry> ToSerializable()
    {
        return _towFactorEntries;
    }

    /// <inheritdoc />
    public void FromSerializable(IEnumerable<BaseEntry> serializable)
    {
        _towFactorEntries = serializable.OfType<TowFactorEntry>().ToList();
    }
}
