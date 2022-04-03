namespace VaultManager.Providers;

public interface IStorageProvider
{
    byte[] ReadAllBytes(string path);
    void CreateDirectory(string path);
    void WriteAllBytes(string path, byte[] data);
}
