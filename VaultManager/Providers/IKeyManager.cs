namespace VaultManager.Providers;

public interface IKeyManager
{
    byte[] LoadKey();
    void SaveKey(byte[] key);
    void CreateKey();
}
