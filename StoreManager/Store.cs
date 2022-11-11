using System.Formats.Tar;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace StoreManager;

public class Store : IStore
{
    private List<StoreEntry> _entries = new List<StoreEntry>();

    /// <inheritdoc />
    public async Task InitializeAsync(Stream stream, CancellationToken cancellationToken)
    {
        TarReader reader = new TarReader(stream);
        while ((await reader.GetNextEntryAsync(cancellationToken: cancellationToken)) is { } entry)
        {
            switch (entry.EntryType)
            {
                case TarEntryType.RegularFile:
                    //$"entry/{storeEntry.Id}.json"
                    if (entry.Name.StartsWith("entry/") && entry.Name.EndsWith(".json") && entry.DataStream is not null)
                    {
                        var storeEntry = await JsonSerializer.DeserializeAsync<StoreEntry>(entry.DataStream, cancellationToken: cancellationToken);
                        if (storeEntry is not null)
                        {
                            _entries.Add(storeEntry);
                        }
                        else
                        {
                            Console.WriteLine($"Failed to deserialize {entry.Name}");
                        }
                    }
                    break;
                default:
                    Console.WriteLine($"Unsupported entry type {entry.EntryType} for {entry.Name}");
                    break;
            }
        }
    }

    /// <inheritdoc />
    public async Task<Stream> GetStreamAsync(CancellationToken cancellationToken)
    {
        MemoryStream stream = new MemoryStream();
        TarWriter writer = new TarWriter(stream, true);

        foreach (var storeEntry in _entries)
        {
            await using var entryStream = new MemoryStream();
            var entry = new GnuTarEntry(TarEntryType.RegularFile, $"entry/{storeEntry.Id}.json");
            await JsonSerializer.SerializeAsync(entryStream, storeEntry, cancellationToken: cancellationToken);
            entryStream.Position = 0;
            entry.DataStream = entryStream;
            await writer.WriteEntryAsync(entry, cancellationToken);
        }
        await writer.DisposeAsync();
        return stream;
    }

    /// <inheritdoc />
    public Task<IStoreEntry?> GetEntryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult<IStoreEntry?>(_entries.FirstOrDefault(e => e.Id == id));
    }

    /// <inheritdoc />
    public Task<IStoreEntry?> GetEntryByNameAsync(string name, CancellationToken cancellationToken)
    {
        return Task.FromResult<IStoreEntry?>(_entries.FirstOrDefault(e => e.Name == name));
    }

    /// <inheritdoc />
    public Task<ICollection<IStoreEntry>> GetEntriesByFieldAsync(string field, CancellationToken cancellationToken)
    {
        return Task.FromResult<ICollection<IStoreEntry>>(_entries.Where(e => e.Fields.Any(x => x.Name == field)).OfType<IStoreEntry>().ToList());
    }

    /// <inheritdoc />
    public Task<ICollection<IStoreEntry>> GetEntriesAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<ICollection<IStoreEntry>>(_entries.OfType<IStoreEntry>().ToList());
    }

    /// <inheritdoc />
    public Task<IStoreEntry> AddEntryAsync(IStoreEntry entry, CancellationToken cancellationToken)
    {
        var storeEntry = new StoreEntry(entry);
        _entries.Add(storeEntry);
        return Task.FromResult((IStoreEntry)storeEntry);
    }

    /// <inheritdoc />
    public Task<IStoreEntry> UpdateEntryAsync(IStoreEntry entry, CancellationToken cancellationToken)
    {
        var storeEntry = _entries.FirstOrDefault(e => e.Id == entry.Id);
        if (storeEntry == null)
        {
            throw new InvalidOperationException("Entry not found");
        }

        storeEntry.Update(entry);
        return Task.FromResult((IStoreEntry)storeEntry);
    }

    /// <inheritdoc />
    public Task<IStoreEntry> DeleteEntryAsync(Guid id, CancellationToken cancellationToken)
    {
        var storeEntry = _entries.FirstOrDefault(e => e.Id == id);
        if (storeEntry == null)
        {
            throw new InvalidOperationException("Entry not found");
        }

        _entries.Remove(storeEntry);
        return Task.FromResult((IStoreEntry)storeEntry);
    }
}
public class StoreEntry : IStoreEntry
{
    public StoreEntry()
    {
        Name = null!;
        Fields = null!;
    }
    public StoreEntry(Guid id, string name)
    {
        Id = id;
        Name = name;
        Created = DateTime.Now;
        Fields = new List<IStoreField>();
    }

    public StoreEntry(IStoreEntry entry)
    {
        Id = entry.Id;
        Name = entry.Name;
        Created = entry.Created;
        Fields = entry.Fields;
    }

    /// <inheritdoc />
    public Guid Id { get; set; }

    /// <inheritdoc />
    public string Name { get; set; }

    /// <inheritdoc />
    public string? Description { get; set; }

    /// <inheritdoc />
    public DateTime Created { get; set; }

    /// <inheritdoc />
    public ICollection<IStoreField> Fields { get; set; }

    public void Update(IStoreEntry entry)
    {
        Name = entry.Name;
        Description = entry.Description;
        foreach (var field in entry.Fields)
        {
            var newField = Fields.FirstOrDefault(f => f.Id == field.Id);
            if (newField != null)
            {
                field.Value = newField.Value;
                field.Name = newField.Name;
            }
            else
            {
                Fields.Add(field);
            }
        }
    }
}
public class StoreField : IStoreField
{
    /// <inheritdoc />
    public Guid Id { get; set; }

    /// <inheritdoc />
    public string Name { get; set; }

    /// <inheritdoc />
    public string Value { get; set; }

    public StoreField(Guid id, string name, string value)
    {
        Id = id;
        Name = name;
        Value = value;
    }

    public StoreField()
    {
        Name = null!;
        Value = null!;
    }
}
