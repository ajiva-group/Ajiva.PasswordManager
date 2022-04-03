using VaultManager.Crypto;
using VaultManager.Providers;

namespace VaultManager;

public class VaultManager
{
    private readonly string _vaultFile;
    private readonly IEncryptionProvider _encryptionProvider;
    private readonly IKeyManager _keyManager;
    private readonly IStorageProvider _storageProvider;
    private readonly IValueSerializationManager _serializationManager;

    public VaultManager(string vaultName, IEncryptionProvider encryptionProvider, IKeyManager keyManager, IStorageProvider storageProvider, IValueSerializationManager serializationManager)
    {
        _vaultFile = Path.Combine(VaultDir, vaultName + ".ajvault");
        _encryptionProvider = encryptionProvider;
        _keyManager = keyManager;
        _storageProvider = storageProvider;
        _serializationManager = serializationManager;
    }

    private void EnsureDirectoryExist()
    {
        _storageProvider.CreateDirectory(VaultDir);
    }

    public string VaultDir { get; set; } = "Vaults";

    public void LoadVault()
    {
        EnsureDirectoryExist();
        var encData = _storageProvider.ReadAllBytes(_vaultFile);
        var key = _keyManager.LoadKey();
        var data = _encryptionProvider.DecryptAsymmetric(encData, key);
        Vault = _serializationManager.DeserializeVault(data);

        Vault.Passwords.Add(Guid.Parse("1E100D0E-BE75-46B7-95D6-EA9FB724E8B7"), new PasswordEntry()
        {
            Description = "Test Description",
            Password = "Test Password",
            Username = "Test Username",
            Notes = "Test Notes",
            Id = Guid.Parse("1E100D0E-BE75-46B7-95D6-EA9FB724E8B7"),
            WebSide = new WebSideEntry
            {
                Description = "Test Description Webside",
                Domain = new Uri("https://www.google.com"),
                Id = Guid.Parse("1E100D0E-BE75-46B7-95D6-EA9FB724E8B7"),
                Settings = new WebSideSettings { },
            }
        });
    }

    public void SaveVault()
    {
        var data = _serializationManager.SerializeVault(Vault);
        var key = _keyManager.LoadKey();
        var encData = _encryptionProvider.EncryptAsymmetric(data, key);
        _storageProvider.WriteAllBytes(_vaultFile, encData);
    }

    public Vault Vault { get; private set; }

    public void CreateVault()
    {
        Vault = new Vault();
        _keyManager.CreateKey();
        SaveVault();
        LoadVault();
    }
}
