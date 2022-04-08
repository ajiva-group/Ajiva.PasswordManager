using System.Security.Cryptography;

namespace VaultManager.Providers;

public interface IKeyManager
{
    /*void LoadRsaKey();
    void SaveRsaKey();*/
    void LoadAesKey();
    void SaveAesKey();
    void CreateKey();
    void LoadKeys();
    void SaveKeysAndClear();
}
