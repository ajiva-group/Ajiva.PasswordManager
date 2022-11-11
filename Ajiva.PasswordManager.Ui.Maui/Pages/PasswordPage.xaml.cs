using Ajiva.PasswordManager.Ui.Maui.Static;
using Ajiva.PasswordManager.Ui.Maui.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using VaultManager;

namespace Ajiva.PasswordManager.Ui.Maui.Pages;

public partial class PasswordPage : ContentPage
{
    private readonly PasswordViewModel _vm;

    public IVaultService VaultService { get; }

    public PasswordPage(IVaultService vaultService)
    {
        _vm = new PasswordViewModel(vaultService);
        BindingContext = _vm;
        InitializeComponent();
        VaultService = vaultService;
    }

    private void Save_Clicked(object sender, EventArgs e)
    {
        VaultService.Vault.SaveVault();
    }

    private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        _vm.Search(PwSearch.Text);
    }

    private void PwCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Deteils.Current = e.CurrentSelection.First() as PasswordVm;
    }
}
