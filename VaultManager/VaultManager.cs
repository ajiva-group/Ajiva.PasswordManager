using VaultManager.Providers;

namespace VaultManager;

public class VaultManager
{
    private string? _vaultName;
    private readonly IKeyManager _keyManager;
    private readonly IVaultLoader _vaultLoader;

    public VaultManager(
        IKeyManager keyManager,
        IVaultLoader vaultLoader)
    {
        _keyManager = keyManager;
        _vaultLoader = vaultLoader;
    }

    public void SetVaultName(string vaultName)
    {
        _vaultName = vaultName;
    }

    public void LoadVault()
    {
        if (_vaultName is null) throw new VaultNotFoundException(_vaultName);
        Vault = _vaultLoader.LoadVault(_vaultName);

        while (Vault.Passwords.Count < 10)
        {
            var id = Guid.NewGuid();
            Vault.Passwords.Add(id, new PasswordEntry {
                Description = "Test Description " + id,
                Password = "PW" + id,
                Username = "Test Username",
                Notes = "Test Notes",
                Id = id,
                WebSide = new WebSideEntry {
                    Description = "Test Description WebSide for " + id,
                    Domain = new Uri("https://www.google.com"),
                    Id = Guid.NewGuid(),
                    Settings = new WebSideSettings { },
                }
            });
        }
        SaveVault();
    }

    public void SaveVault()
    {
        _vaultLoader.SaveVault(Vault);
    }

    public Vault Vault { get; private set; }

    public PasswordEntry Update(PasswordEntry passwordEntry)
    {
        var res = Vault.Update(passwordEntry);
        SaveVault();
        return res;
    }

    public void CreateVault()
    {
        if (_vaultName is null) throw new VaultNotFoundException(_vaultName);
        Vault = _vaultLoader.CreateVault(_vaultName);
    }
}
