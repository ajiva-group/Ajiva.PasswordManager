using System.Security.Cryptography;
using System.Text;
using VaultManager.Providers;

namespace VaultManager.Crypto;

public class KeyManager : IKeyManager
{
    private IStorageProvider _storageProvider;
    private readonly IVaultConfig _vaultConfig;

    public KeyManager(IStorageProvider storageProvider, IVaultConfig vaultConfig)
    {
        _storageProvider = storageProvider;
        _vaultConfig = vaultConfig;
        _storageProvider.CreateDirectory(_vaultConfig.KeyDir);
    }

    /// <inheritdoc />
    public VaultKey CreateKey(string name)
    {
        var key = new VaultKey(VaultKey.GenerateRandomBlob(), name);
        SaveKey(key);
        return key;
    }

    /// <inheritdoc />
    public VaultKey LoadKey(string name)
    {
        return new VaultKey(
            _storageProvider.ReadAllBytes(
                GetKeyFile(name)
            ),
            name
        );
    }

    private string GetKeyFile(string name) => $"{_vaultConfig.KeyDir}/{name}.key";

    /// <inheritdoc />
    public void SaveKey(VaultKey key)
    {
        _storageProvider.WriteAllBytes(
            GetKeyFile(key.Name),
            key.ExportKeyBlob()
        );
    }
}
