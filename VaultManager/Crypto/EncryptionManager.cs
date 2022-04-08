using System.Security.Cryptography;
using System.Text;
using VaultManager.Providers;

namespace VaultManager.Crypto;

public class EncryptionManager : IEncryptionProvider
{
    //private byte[] StaticIV = new byte[16] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x15, 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7 };
    //private byte[] StaticIV2 = new byte[16] { 0xF2, 0xD2, 0xB2, 0x92, 0x72, 0x52, 0x32, 0x12, 0xA4, 0x6C, 0x4C, 0x22, 0x0C, 0x8A, 0x6C, 0x4C };
    //private RSA  _rsa;
    private Aes _aes;

    public EncryptionManager()
    {
        Clear();
    }

    /// <inheritdoc />
    public byte[] DecryptSymmetric(byte[] encrypted)
    {
        using var decryptor = _aes.CreateDecryptor();
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);

        cs.Write(encrypted, 0, encrypted.Length);
        cs.FlushFinalBlock();

        return ms.ToArray();
    }

    /// <inheritdoc />
    public byte[] EncryptSymmetric(byte[] data)
    {
        using var encryptor = _aes.CreateEncryptor();
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);

        cs.Write(data, 0, data.Length);
        cs.FlushFinalBlock();

        return ms.ToArray();
    }

    /*
    /// <inheritdoc />
    public byte[] DecryptAsymmetric(byte[] encrypted)
    {
        return _rsa.Decrypt(encrypted, RSAEncryptionPadding.Pkcs1);
    }

    /// <inheritdoc />
    public byte[] EncryptAsymmetric(byte[] data)
    {
        return _rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
    }

    /// <inheritdoc />
    public byte[] SingAsymmetric(byte[] data)
    {
        return _rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    /// <inheritdoc />
    public bool VerifyAsymmetric(byte[] data, byte[] signature)
    {
        return _rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    /// <inheritdoc />
    public void LoadAsymmetricKey(ReadOnlySpan<char> passwordBytes, ReadOnlySpan<char> source)
    {
        _rsa.ImportFromEncryptedPem(source, passwordBytes);
    }

    /// <inheritdoc />
    public ReadOnlySpan<byte> ExportAsymmetricKey(ReadOnlySpan<char> passwordBytes)
    {
        var crypto = new RSACryptoServiceProvider();
        crypto.ImportParameters(_rsa.ExportParameters(true));

        var key = crypto.ExportCspBlob(true);
        
        
        ECDsa ecDsa = ECDsa.Create();
        ecDsa.ExportEncryptedPkcs8PrivateKey()
    }
    */

    /// <inheritdoc />
    public byte[] ExportSymmetricKey()
    {
        var joint = new byte[AES_KEY_SIZE + AES_IV_SIZE];
        _aes.Key.CopyTo(joint, 0);
        _aes.IV.CopyTo(joint, AES_KEY_SIZE);
        //return _rsa.Encrypt(joint, RSAEncryptionPadding.OaepSHA512);
        return joint;
    }

    /// <inheritdoc />
    public void Clear()
    {
        //_rsa?.Dispose();
        _aes?.Dispose();
        _aes = Aes.Create();
        _aes.KeySize = AES_KEY_SIZE_BIT;
        _aes.GenerateKey();
        _aes.GenerateIV();
        //_rsa = RSA.Create();
    }

    public const int AES_KEY_SIZE_BIT = 256;
    public const int AES_KEY_SIZE = AES_KEY_SIZE_BIT / 8;
    public const int AES_IV_SIZE = AES_KEY_SIZE / 2;

    /// <inheritdoc />
    public void LoadSymmetricKey(byte[] data)
    {
        var joint = data;//_rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA512);
        _aes.Key = joint.AsSpan(0, AES_KEY_SIZE).ToArray();
        _aes.IV = joint.AsSpan(AES_KEY_SIZE).ToArray();
    }
}
