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
        _keyManager.LoadKeys();
        var data = _encryptionProvider.DecryptSymmetric(encData);
        Vault = _serializationManager.DeserializeVault(data);
        _keyManager.SaveKeysAndClear();

        while (Vault.Passwords.Count < 10)
        {
            var id = Guid.NewGuid();
            Vault.Passwords.Add(id, new PasswordEntry
            {
                Description = "Test Description " + id,
                Password = "PW" + id,
                Username = "Test Username",
                Notes = "Test Notes",
                Id = id,
                WebSide = new WebSideEntry
                {
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
        var data = _serializationManager.SerializeVault(Vault);
        _keyManager.LoadKeys();
        var encData = _encryptionProvider.EncryptSymmetric(data);
        _storageProvider.WriteAllBytes(_vaultFile, encData);
        _keyManager.SaveKeysAndClear();
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
