using VaultManager.Models;

namespace VaultManager.Vaults;

public class WebSideVault :ISerializableVault
{
    private  List<WebSideEntry> _webSideEntries;

    public WebSideVault()
    {
        _webSideEntries = new List<WebSideEntry>();
    }

    public void AddWebSide(WebSideEntry webSideEntry)
    {
        _webSideEntries.Add(webSideEntry);
    }

    /// <inheritdoc />
    public IEnumerable<BaseEntry> ToSerializable()
    {
        return _webSideEntries;
    }

    /// <inheritdoc />
    public void FromSerializable(IEnumerable<BaseEntry> serializable)
    {
        _webSideEntries = serializable.OfType<WebSideEntry>().ToList();
    }
}
