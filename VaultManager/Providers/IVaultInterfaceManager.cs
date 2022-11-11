using System.Security.Cryptography;

namespace VaultManager.Providers;

public interface IVaultInterfaceManager
{
    string[] GetVaults();
    void CreateVault(string vaultName);
}
public class VaultInterfaceManager : IVaultInterfaceManager
{
    private readonly IStorageProvider _storageProvider;
    private readonly IVaultConfig _vaultConfig;
    private readonly IVaultLoader _vaultLoader;

    public VaultInterfaceManager(IStorageProvider storageProvider, IVaultConfig vaultConfig, IVaultLoader vaultLoader)
    {
        _storageProvider = storageProvider;
        _vaultConfig = vaultConfig;
        _vaultLoader = vaultLoader;
    }

    /// <inheritdoc />
    public string[] GetVaults()
    {
        return _storageProvider.ListFiles(_vaultConfig.VaultDir).Select(Path.GetFileNameWithoutExtension).ToArray()!;
    }

    public void CreateVault(string vaultName)
    {
        _vaultLoader.CreateVault(vaultName);
    }
}
