using System.Collections.ObjectModel;
using StoreManager;

namespace Ajiva.PasswordManager.Maui.ViewModels;

public class PasswordViewModel
{
    public PasswordViewModel(IStore store)
    {
        Items = new ObservableCollection<IStoreEntry>(store.GetEntriesAsync(CancellationToken.None).Result);
    }

    public ObservableCollection<IStoreEntry> Items { get; }
    public IStoreEntry Selected { get; set; }
}
