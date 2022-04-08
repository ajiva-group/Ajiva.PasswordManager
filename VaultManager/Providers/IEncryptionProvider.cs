using System.Threading.Channels;

namespace VaultManager.Providers;

public interface IEncryptionProvider
{
    byte[] DecryptSymmetric(byte[] encrypted);

    byte[] EncryptSymmetric(byte[] data);

    /*byte[] DecryptAsymmetric(byte[] encrypted);

    byte[] EncryptAsymmetric(byte[] data);
    byte[] SingAsymmetric(byte[] data);
    bool VerifyAsymmetric(byte[] data, byte[] signature);
    void LoadAsymmetricKey(ReadOnlySpan<char> passwordBytes, ReadOnlySpan<char> source);
    ReadOnlySpan<char> ExportAsymmetricKey(ReadOnlySpan<char> passwordBytes);*/
    public void LoadSymmetricKey(byte[] data);
    public byte[] ExportSymmetricKey();
    void Clear();
}
