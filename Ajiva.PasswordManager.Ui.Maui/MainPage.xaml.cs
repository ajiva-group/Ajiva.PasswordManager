using Ajiva.PasswordManager.Ui.Maui.Static;
using VaultManager;
using VaultManager.Crypto;
using VaultManager.Providers;

namespace Ajiva.PasswordManager.Ui.Maui
{
    public partial class MainPage : ContentPage
    {
        private readonly IVaultInterfaceManager _vaultInterfaceManager;
        public event OnLoginEventHandler OnLogin;

        public delegate void OnLoginEventHandler(string vaultName, string password);

        public MainPage(IVaultInterfaceManager vaultInterfaceManager)
        {
            _vaultInterfaceManager = vaultInterfaceManager;
            Vaults = new(_vaultInterfaceManager.GetVaults());

            _page = new AddVaultPage(_vaultInterfaceManager);
            _page.OnCreated += name =>
            {
                Vaults.Add(name);
                Content = _save;
            };
            InitializeComponent();
            this.BindingContext = this;
        }

        public List<string> Vaults { get; set; }

        private AddVaultPage _page;
        private Microsoft.Maui.Controls.View _save;

        private void OnUnlockClicked(object sender, EventArgs e)
        {
            if(VaultSelect.SelectedIndex == -1)
            {
                Error.Text = "Please select a vault";
                return;
            }    
            OnLogin?.Invoke((string)VaultSelect.SelectedItem, PasswordInput.Text);
        }

        private void AddVaultClick(object sender, EventArgs e)
        {
            _save = Content;
            Content = _page.Content;
        }
    }
}
