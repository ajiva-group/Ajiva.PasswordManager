using Ajiva.PasswordManager.Maui.ViewModels;
using StoreManager;

namespace Ajiva.PasswordManager.Maui.Pages;

public partial class Passwords : ContentPage
{
    private readonly IStore _store;

    public Passwords(IStore store)
	{
		InitializeComponent();
        _store = store;

        BindingContext = new PasswordViewModel(_store);
    }

    private void PwSearch_SearchButtonPressed(object sender, EventArgs e)
    {

    }

    private void PwCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ((PasswordViewModel)BindingContext).Selected = (IStoreEntry)e.CurrentSelection.FirstOrDefault();
    }
}
