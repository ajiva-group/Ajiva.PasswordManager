namespace VaultManager.Providers;

public interface IEncryptionProvider
{
    byte[] DecryptSymmetric(byte[] encrypted, byte[] key);

    byte[] EncryptSymmetric(byte[] data, byte[] key);

    byte[] GenerateSymmetricKey(string salt);

    byte[] DecryptAsymmetric(byte[] encrypted, byte[] blob);

    byte[] EncryptAsymmetric(byte[] data, byte[] blob);

    byte[] GenerateAsymmetricKey();

    byte[] SingAsymmetric(byte[] data, byte[] blob);
    bool VerifyAsymmetric(byte[] data, byte[] blob, byte[] signature);
}
