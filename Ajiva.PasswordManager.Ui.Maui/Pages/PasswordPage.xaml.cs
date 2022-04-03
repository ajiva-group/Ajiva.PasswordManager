using Ajiva.PasswordManager.Ui.Maui.Static;
using Ajiva.PasswordManager.Ui.Maui.ViewModels;
using VaultManager;

namespace Ajiva.PasswordManager.Ui.Maui.Pages;

public partial class PasswordPage : ContentPage
{


    public PasswordPage()
    {
        BindingContext = new PasswordViewModel();

        InitializeComponent();
    }

    private void Save_Clicked(object sender, EventArgs e)
    {
        StaticData.Vault.SaveVault();
    }
}
