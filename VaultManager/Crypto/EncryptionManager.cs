using System.Security.Cryptography;
using System.Text;
using VaultManager.Providers;

namespace VaultManager.Crypto;

public class EncryptionManager : IEncryptionProvider
{
    /// <inheritdoc />
    public byte[] DecryptSymmetric(VaultKey key, byte[] encryptedData)
    {
        var aes = key.CreateAes();
        using var decryptor = aes.CreateDecryptor();
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);

        cs.Write(encryptedData, 0, encryptedData.Length);
        cs.FlushFinalBlock();

        return ms.ToArray();
    }

    /// <inheritdoc />
    public byte[] EncryptSymmetric(VaultKey key, byte[] data)
    {
        var aes = key.CreateAes();
        using var encryptor = aes.CreateEncryptor();
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);

        cs.Write(data, 0, data.Length);
        cs.FlushFinalBlock();

        return ms.ToArray();
    }
}
