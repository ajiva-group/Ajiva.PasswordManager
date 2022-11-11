namespace StoreManager;

public interface IStore
{
    Task InitializeAsync(Stream stream,CancellationToken cancellationToken);
    Task<Stream> GetStreamAsync(CancellationToken cancellationToken);
    
    Task<IStoreEntry?> GetEntryByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IStoreEntry?> GetEntryByNameAsync(string name, CancellationToken cancellationToken);
    Task<ICollection<IStoreEntry>> GetEntriesByFieldAsync(string field, CancellationToken cancellationToken);
    Task<ICollection<IStoreEntry>> GetEntriesAsync(CancellationToken cancellationToken);
    Task<IStoreEntry> AddEntryAsync(IStoreEntry entry, CancellationToken cancellationToken);
    Task<IStoreEntry> UpdateEntryAsync(IStoreEntry entry, CancellationToken cancellationToken);
    Task<IStoreEntry> DeleteEntryAsync(Guid id, CancellationToken cancellationToken);
    
}
public interface IStoreEntry
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; }
    public ICollection<IStoreField> Fields { get; set; }
}
public interface IStoreField
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
}

public static class FieldNames
{
    public const string Name = "Url";
    public const string Notes = "Notes";
    public const string Username = "Username";
    public const string Password = "Password";
    public const string Email = "Created";
    public const string Tags = "Tags";
}
