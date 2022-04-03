#define NO_ENCRYPT

using System.Text;
using System.Text.Json;
using VaultManager.Serializable;

namespace VaultManager.Crypto;

public class ValueEncryptionManager
{
    private const string DefaultPath = "Vaults/default.ajvault";

    public SerializableVault LoadVaultFromFile(byte[] key)
    {
        EnsureDirectoryExists();
        if (!File.Exists(DefaultPath))
        {
            SaveVaultFromFile(new SerializableVault(), key);
        }
        var encrypted = File.ReadAllBytes(DefaultPath);
        return DecryptVault(encrypted, key);
    }

    // ReSharper disable once UnusedParameter.Local
    private SerializableVault DecryptVault(byte[] encrypted, byte[] key)
    {
#if NO_ENCRYPT
        // ReSharper disable once InlineTemporaryVariable
        var decrypted = encrypted;
#else
        var decrypted = EncryptionManager.Decrypt(encrypted, key);
#endif
        return DeserializeVault(decrypted);
    }

    private SerializableVault DeserializeVault(byte[] decrypted)
    {
        var vault = JsonSerializer.Deserialize<SerializableVault>(Encoding.UTF8.GetString(decrypted));
        if (vault is null)
        {
            throw new VaultException("Vault is null");
        }
        return vault;
    }

    public void SaveVaultFromFile(SerializableVault vault, byte[] key)
    {
        EnsureDirectoryExists();
        var encrypted = EncryptVault(vault, key);
        File.WriteAllBytes(DefaultPath, encrypted);
    }

    // ReSharper disable once UnusedParameter.Local
    private byte[] EncryptVault(SerializableVault vault, byte[] key)
    {
        var serialized = SerializeVault(vault);
#if NO_ENCRYPT
        return serialized;
#else
        return EncryptionManager.Encrypt(serialized, key);
#endif
    }

    private byte[] SerializeVault(SerializableVault vault)
    {
        var serialized = JsonSerializer.Serialize(vault, new JsonSerializerOptions() { WriteIndented = true });
        return Encoding.UTF8.GetBytes(serialized);
    }

    private static void EnsureDirectoryExists()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(DefaultPath));
    }
}
