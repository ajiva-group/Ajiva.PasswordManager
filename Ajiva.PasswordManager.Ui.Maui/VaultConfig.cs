using VaultManager.Providers;

namespace Ajiva.PasswordManager.Ui.Maui;

public class VaultConfig  : IVaultConfig
{
    /// <inheritdoc />
    public string VaultDir => "Vaults";

    /// <inheritdoc />
    public string KeyDir => "Keys";
}
