using Ajiva.PasswordManager.Ui.Maui.Pages;
using Ajiva.PasswordManager.Ui.Maui.Static;
using VaultManager.Crypto;

namespace Ajiva.PasswordManager.Ui.Maui;

public partial class App : Application
{
    Page _realPage;

    public App()
    {
        InitializeComponent();

        if (DeviceInfo.Idiom == DeviceIdiom.Phone)
            Shell.Current.CurrentItem = PhoneTabs;

        _realPage = MainPage;

        var mainPage = new MainPage();
        mainPage.OnLogin += MainPage_OnLogin;
        MainPage = mainPage;

        Routing.RegisterRoute("settings", typeof(SettingsPage));
    }

    private void MainPage_OnLogin(object sender, string e)
    {
        StaticData.Load(e);

        MainPage = _realPage;
    }
}
