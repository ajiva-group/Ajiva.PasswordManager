using System.Threading.Channels;

namespace VaultManager.Providers;

public interface IEncryptionProvider
{
    byte[] DecryptSymmetric(VaultKey key, byte[] encryptedData);

    byte[] EncryptSymmetric(VaultKey key, byte[] data);
}
