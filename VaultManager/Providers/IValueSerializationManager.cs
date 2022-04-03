namespace VaultManager.Providers;

public interface IValueSerializationManager
{
    Vault DeserializeVault(byte[] data);
    byte[] SerializeVault(Vault vault);
}