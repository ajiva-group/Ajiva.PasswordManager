using VaultManager.Models;

namespace VaultManager.Vaults;

public class FileVault : ISerializableVault
{
    private List<FileEntry> _fileEntries;

    public FileVault()
    {
        _fileEntries = new List<FileEntry>();
    }

    public void AddFileEntry(FileEntry fileEntry)
    {
        _fileEntries.Add(fileEntry);
    }

    /// <inheritdoc />
    public IEnumerable<BaseEntry> ToSerializable()
    {
        return _fileEntries;
    }

    /// <inheritdoc />
    public void FromSerializable(IEnumerable<BaseEntry> serializable)
    {
        _fileEntries = serializable.OfType<FileEntry>().ToList();
    }
}
