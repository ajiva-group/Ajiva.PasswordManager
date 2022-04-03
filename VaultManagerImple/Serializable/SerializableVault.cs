using VaultManager.Models;

namespace VaultManager.Serializable;

public class SerializableVault
{
    public Dictionary<Guid, object> CurrentEntries { get; set; } = new Dictionary<Guid, object>();
    //public List<EntryHistory> History { get; set; }

    public void Add(IEnumerable<BaseEntry> enumerable)
    {
        foreach (var entry in enumerable)
        {
            CurrentEntries.Add(entry.Identifier, entry);
        }
    }

    public IEnumerable<BaseEntry> Of<T>() where T : BaseEntry
    {
        return CurrentEntries.Values.OfType<T>();
    }
}
public record EntryHistory(Guid EntryId, DateTime Date, BaseEntry? Before, BaseEntry? After);
