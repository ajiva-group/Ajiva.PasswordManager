namespace VaultManager;

public class VaultNotFound : Exception
{
    public VaultNotFound(string vaultName) : base("The Specified Vault was not found: " + vaultName)
    {
    }
}
