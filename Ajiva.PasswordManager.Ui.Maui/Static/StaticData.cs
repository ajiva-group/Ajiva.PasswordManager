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
    internal class StaticData
    {
        public static VaultManager.VaultManager Vault { get; set; }

        static StaticData()
        {
            var store = IsolatedStorageFile.GetUserStoreForApplication();
        }

        public static void Load(string password)
        {
            IEncryptionProvider encryptionProvider = new EncryptionManager();
            IStorageProvider storageProvider = new MauiStorageProvider();
            IValueSerializationManager serializationManager = new ValueSerializationManager();
            IKeyManager keyManager = new KeyManager(encryptionProvider, storageProvider, "default", password);

            Vault = new VaultManager.VaultManager("default", encryptionProvider, keyManager, storageProvider, serializationManager);


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
