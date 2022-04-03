using Ajiva.PasswordManager.Ui.Maui.Static;
using VaultManager;
using VaultManager.Crypto;

namespace Ajiva.PasswordManager.Ui.Maui
{
    public partial class MainPage : ContentPage
    {
        public event EventHandler<string> OnLogin;
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
            //CounterLabel.Text = $"Current count: {count}";

            //SemanticScreenReader.Announce(CounterLabel.Text);
        }

        private void OnUnlockClicked(object sender, EventArgs e)
        {
            OnLogin?.Invoke(this, PasswordInput.Text);
        }
    }
}