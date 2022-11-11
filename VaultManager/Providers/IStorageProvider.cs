namespace VaultManager.Providers;

public interface IStorageProvider
{
    byte[] ReadAllBytes(string path);
    string ReadAllText(string path);
    void CreateDirectory(string path);
    void WriteAllBytes(string path, byte[] data);
    void WriteAllText(string path, string data);
    string[] ListFiles(string directory);
    string[] ListDirectories(string directory);
}
