using System.Security.Cryptography;
using VaultManager.Providers;

namespace VaultManager.Crypto;

public class EncryptionManager : IEncryptionProvider
{
    private byte[] StaticIV = new byte[16] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x15, 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7 };
    private byte[] StaticIV2 = new byte[16] { 0xF2, 0xD2, 0xB2, 0x92, 0x72, 0x52, 0x32, 0x12, 0xA4, 0x6C, 0x4C, 0x22, 0x0C, 0x8A, 0x6C, 0x4C };

    /// <summary>
    /// Encrypt Using RSA
    /// </summary>
    /// <param name="encrypted"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public byte[] Decrypt(byte[] encrypted, byte[] key)
    {
        using var rsa = new RSACryptoServiceProvider();

        rsa.ImportCspBlob(key);
        return rsa.Decrypt(encrypted, false);
    }

    /// <summary>
    /// Encrypt Using RSA
    /// </summary>
    /// <param name="data"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public byte[] Encrypt(byte[] data, byte[] key)
    {
        using var rsa = new RSACryptoServiceProvider();
        rsa.ImportCspBlob(key);

        return rsa.Encrypt(data, false);
    }

    /// <summary>
    /// Generate Random RSA Key
    /// </summary>
    /// <returns></returns>
    public byte[] GenerateKey()
    {
        using var rsa = CreateRsa();
        return rsa.ExportCspBlob(true);
    }

    private RSACryptoServiceProvider CreateRsa() => new RSACryptoServiceProvider(8192);

    /// <inheritdoc />
    public byte[] DecryptSymmetric(byte[] encrypted, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;

        return aes.DecryptCfb(encrypted, StaticIV);
    }

    /// <inheritdoc />
    public byte[] EncryptSymmetric(byte[] data, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;

        return aes.EncryptCfb(data, StaticIV);
    }

    /// <inheritdoc />
    public byte[] GenerateSymmetricKey(string salt)
    {
        var pwdGen = new Rfc2898DeriveBytes(salt, StaticIV2, 8192, HashAlgorithmName.SHA256);
        return pwdGen.GetBytes(32);
    }

    /// <inheritdoc />
    public byte[] DecryptAsymmetric(byte[] encrypted, byte[] blob)
    {
        using var rsa = new RSACryptoServiceProvider();
        rsa.ImportCspBlob(blob);

        return rsa.Decrypt(encrypted, false);
    }

    /// <inheritdoc />
    public byte[] EncryptAsymmetric(byte[] data, byte[] blob)
    {
        using var rsa = new RSACryptoServiceProvider();
        rsa.ImportCspBlob(blob);

        return rsa.Encrypt(data, false);
    }

    /// <inheritdoc />
    public byte[] GenerateAsymmetricKey()
    {
        using var rsa = new RSACryptoServiceProvider(8192);
        return rsa.ExportCspBlob(true);
    }

    /// <inheritdoc />
    public byte[] SingAsymmetric(byte[] data, byte[] blob)
    {
        using var rsa = new RSACryptoServiceProvider();
        rsa.ImportCspBlob(blob);

        return rsa.SignData(data, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
    }

    /// <inheritdoc />
    public bool VerifyAsymmetric(byte[] data, byte[] blob, byte[] signature)
    {
        using var rsa = new RSACryptoServiceProvider();
        rsa.ImportCspBlob(blob);

        return rsa.VerifyData(data, signature, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
    }
}
