using System.Security.Cryptography;

namespace VaultManager.Providers;

public interface IKeyManager
{
    VaultKey CreateKey(string name);
    VaultKey LoadKey(string name);
    
    void SaveKey(VaultKey key);
}
public class VaultKey : IDisposable
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public VaultKey(byte[] blob, string name)
    {
        Name = name;
        _key = blob.AsSpan(0, AES_KEY_SIZE).ToArray();
        _iv = blob.AsSpan(AES_KEY_SIZE).ToArray();
    }

    public const int AES_KEY_SIZE_BIT = 256;
    public const int AES_KEY_SIZE = AES_KEY_SIZE_BIT / 8;
    public const int AES_IV_SIZE = AES_KEY_SIZE / 2;
    public const int BLOB_SIZE = AES_KEY_SIZE + AES_IV_SIZE;
    public static byte[] GenerateRandomBlob() => RandomNumberGenerator.GetBytes(BLOB_SIZE);

    public byte[] ExportKeyBlob()
    {
        var joint = new byte[BLOB_SIZE];
        _key.CopyTo(joint, 0);
        _iv.CopyTo(joint, AES_KEY_SIZE);
        return joint;
    }

    public string Name { get; }

    public Aes CreateAes()
    {
        var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        return aes;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Array.Clear(_key);
        Array.Clear(_iv);
    }
}
