namespace VaultManager.Providers;

public interface IVaultLoader
{
    Vault CreateVault(string name);
    Vault LoadVault(string name);
    void SaveVault(Vault vault);
}
public class VaultLoader : IVaultLoader
{
    private readonly IKeyManager _keyManager;
    private readonly IVaultConfig _vaultConfig;
    private readonly IValueSerializationManager _serializationManager;
    private readonly IStorageProvider _storageProvider;
    private readonly IEncryptionProvider _encryptionProvider;

    public VaultLoader(IKeyManager keyManager, IVaultConfig vaultConfig, IValueSerializationManager serializationManager, IStorageProvider storageProvider, IEncryptionProvider encryptionProvider)
    {
        _keyManager = keyManager;
        _vaultConfig = vaultConfig;
        _serializationManager = serializationManager;
        _storageProvider = storageProvider;
        _encryptionProvider = encryptionProvider;

        _storageProvider.CreateDirectory(_vaultConfig.VaultDir);
    }

    /// <inheritdoc />
    public Vault LoadVault(string name)
    {
        return _serializationManager.DeserializeVault(
            _encryptionProvider.DecryptSymmetric(
                _keyManager.LoadKey(name),
                _storageProvider.ReadAllBytes(
                    GetVaultFile(name)
                )
            )
        );
    }

    /// <inheritdoc />
    public void SaveVault(Vault vault)
    {
        _storageProvider.WriteAllBytes(
            GetVaultFile(vault.Name),
            _encryptionProvider.EncryptSymmetric(
                _keyManager.LoadKey(vault.Name),
                _serializationManager.SerializeVault(vault)
            )
        );
    }

    /// <inheritdoc />
    public Vault CreateVault(string name)
    {
        _keyManager.CreateKey(name);
        SaveVault(new Vault(name));
        return LoadVault(name);
    }

    private string GetVaultFile(string name)
    {
        return $"{_vaultConfig.VaultDir}/{name}.ajVault";
    }
}
