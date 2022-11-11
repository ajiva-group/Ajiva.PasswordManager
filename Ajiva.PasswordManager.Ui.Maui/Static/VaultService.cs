using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultManager;
using VaultManager.Crypto;
using VaultManager.Providers;

namespace Ajiva.PasswordManager.Ui.Maui.Static
{
    public interface IVaultService
    {
        VaultManager.VaultManager Vault { get; set; }
        void Load(string name, string password);
    }
    public class VaultService : IVaultService
    {
        public VaultManager.VaultManager Vault { get; set; }

        public VaultService(IKeyManager keyManager, IVaultLoader vaultLoader)
        {
            
            Vault = new VaultManager.VaultManager(keyManager, vaultLoader);
        }

        public void Load(string name, string password)
        {
            Vault.SetVaultName(name);

            try
            {
                Vault.LoadVault();
            }
            catch (VaultNotFound e)
            {
                Vault.CreateVault();
            }
        }
    }
}
