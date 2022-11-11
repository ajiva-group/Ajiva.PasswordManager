namespace VaultManager;

public class VaultNotFoundException : Exception
{
    public VaultNotFoundException(string? vaultName) : base($"Vault {vaultName} not found")
    {
    }
}