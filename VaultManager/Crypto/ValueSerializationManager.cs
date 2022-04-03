#define NO_ENCRYPT

using System.Text;
using System.Text.Json;
using VaultManager.Providers;

namespace VaultManager.Crypto;

public class ValueSerializationManager : IValueSerializationManager
{
    public Vault DeserializeVault(byte[] data)
    {
        var vault = JsonSerializer.Deserialize<Vault>(Encoding.UTF8.GetString(data));
        if (vault is null)
        {
            throw new Exception("Vault is null");
        }
        return vault;
    }

    public byte[] SerializeVault(Vault vault)
    {
        var serialized = JsonSerializer.Serialize(vault, new JsonSerializerOptions() { WriteIndented = true });
        return Encoding.UTF8.GetBytes(serialized);
    }
}
