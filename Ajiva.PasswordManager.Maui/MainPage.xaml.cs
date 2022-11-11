using Ajiva.PasswordManager.Maui.Pages;
using StoreManager;

namespace Ajiva.PasswordManager.Maui;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
    }


    private void OnLoadClicked(object sender, EventArgs e)
    {
        var stream = File.OpenRead(@"U:\source\ajiva-group\AjivaPassowrdManager\StoreCli\bin\Debug\net7.0\test.tar");
        var store = new Store();
        store.InitializeAsync(stream, CancellationToken.None).Wait();
        stream.Close();
        Navigation.PushAsync(new Passwords(store));
    }
}
