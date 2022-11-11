using VaultManager.Providers;

namespace Ajiva.PasswordManager.Ui.Maui;

public class AddVaultPage : ContentPage
{
    private readonly IVaultInterfaceManager _vaultInterfaceManager;
    private readonly Entry _nameEntry;
    private readonly Label _errorLabel;

    public event Action<string> OnCreated;

    public AddVaultPage(IVaultInterfaceManager vaultInterfaceManager)
    {
        _vaultInterfaceManager = vaultInterfaceManager;
        Content = new StackLayout
        {
            Children = {
                (_errorLabel = new Label { Text = "" }),
                (_nameEntry = new Entry { Placeholder = "Vault Name"}),
                new Button { Text = "Create", Command = new Command(CreateVault) }
            }
        };
    }

    private void CreateVault()
    {
        var name = _nameEntry.Text;
        if (string.IsNullOrEmpty(name))
            _errorLabel.Text = "Please enter a name for the vault";
        else _vaultInterfaceManager.CreateVault(name);
        OnCreated?.Invoke(name);
    }
}
