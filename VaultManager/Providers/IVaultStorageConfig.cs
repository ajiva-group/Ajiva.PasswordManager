namespace VaultManager.Providers;

public interface IVaultConfig
{
    string VaultDir { get; }
    string KeyDir { get; }
}
