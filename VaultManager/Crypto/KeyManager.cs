using System.Security.Cryptography;
using System.Text;
using VaultManager.Providers;

namespace VaultManager.Crypto;

public class KeyManager : IKeyManager
{
    private IEncryptionProvider _encryptionProvider;
    private IStorageProvider _storageProvider;
    private readonly string _password;
    private readonly string _keyRsaPath;
    private readonly string _keyAesPath;

    public string KeyDir { get; set; } = "Keys";

    public KeyManager(IEncryptionProvider encryptionProvider, IStorageProvider storageProvider, string keyName, string password)
    {
        _encryptionProvider = encryptionProvider;
        _storageProvider = storageProvider;
        _password = password;
        _keyRsaPath = Path.Combine(KeyDir, keyName + ".rsa.key");
        _keyAesPath = Path.Combine(KeyDir, keyName + ".aes.key");
    }

    private void EnsureDirectoryExist()
    {
        _storageProvider.CreateDirectory(KeyDir);
    }

    /*
    /// <inheritdoc />
    public void LoadRsaKey()
    {
        var source = _storageProvider.ReadAllText(_keyRsaPath);
        _encryptionProvider.LoadAsymmetricKey(_password, source);
    }

    /// <inheritdoc />
    public void SaveRsaKey()
    {
        var rsaBytes = _encryptionProvider.ExportAsymmetricKey(_password);
        _storageProvider.WriteAllBytes(_keyRsaPath, rsaBytes);
    }
    */

    /// <inheritdoc />
    public void LoadAesKey()
    {
        var data = _storageProvider.ReadAllBytes(_keyAesPath);
        _encryptionProvider.LoadSymmetricKey(data);
    }

    /// <inheritdoc />
    public void SaveAesKey()
    {
        var aes = _encryptionProvider.ExportSymmetricKey();
        _storageProvider.WriteAllBytes(_keyAesPath, aes);
    }

    /// <inheritdoc />
    public void CreateKey()
    {
        SaveKeysAndClear();
        LoadKeys();
    }

    /// <inheritdoc />
    public void LoadKeys()
    {
        EnsureDirectoryExist();
        //LoadRsaKey();
        LoadAesKey();
    }

    /// <inheritdoc />
    public void SaveKeysAndClear()
    {
        EnsureDirectoryExist();
        //SaveRsaKey();
        SaveAesKey();
        _encryptionProvider.Clear();
    }
}
