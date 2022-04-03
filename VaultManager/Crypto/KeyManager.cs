using VaultManager.Providers;

namespace VaultManager.Crypto;

public class KeyManager : IKeyManager
{
    private IEncryptionProvider _encryptionProvider;
    private IStorageProvider _storageProvider;
    private readonly string _keyPath;
    private readonly byte[] _key;

    public string KeyDir { get; set; } = "Keys";

    public KeyManager(IEncryptionProvider encryptionProvider, IStorageProvider storageProvider, string keyName, string password)
    {
        _encryptionProvider = encryptionProvider;
        _storageProvider = storageProvider;
        _keyPath = Path.Combine(KeyDir, keyName + ".key");
        _key = _encryptionProvider.GenerateSymmetricKey(password);
    }

    public byte[] LoadKey()
    {
        EnsureDirectoryExist();
        var keyBytes = _storageProvider.ReadAllBytes(_keyPath);
        return _encryptionProvider.DecryptSymmetric(keyBytes, _key);
    }

    private void EnsureDirectoryExist()
    {
        _storageProvider.CreateDirectory(KeyDir);
    }

    public void SaveKey(byte[] key)
    {
        EnsureDirectoryExist();
        var keyBytes = _encryptionProvider.EncryptSymmetric(key, _key);
        _storageProvider.WriteAllBytes(_keyPath, keyBytes);
    }

    /// <inheritdoc />
    public void CreateKey()
    {
        SaveKey(_encryptionProvider.GenerateAsymmetricKey());
    }
}
