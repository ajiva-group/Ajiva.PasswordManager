using Ajiva.PasswordManager.Ui.Maui.Pages;
using Ajiva.PasswordManager.Ui.Maui.Static;
using VaultManager.Crypto;
using VaultManager.Providers;

namespace Ajiva.PasswordManager.Ui.Maui;

public partial class App : Application
{
    private readonly IVaultService _vaultService;
    Page _realPage;

    public App(IVaultService vaultService, IVaultInterfaceManager vaultInterfaceManager)
    {
        _vaultService = vaultService;
        InitializeComponent();

        if (DeviceInfo.Idiom == DeviceIdiom.Phone)
            Shell.Current.CurrentItem = PhoneTabs;

        _realPage = MainPage;

        var mainPage = new MainPage(vaultInterfaceManager);
        mainPage.OnLogin += MainPage_OnLogin;
        MainPage = mainPage;
    }

    private void MainPage_OnLogin(string vaultName, string password)
    {
        _vaultService.Load(vaultName, password);

        MainPage = _realPage;
    }
}
