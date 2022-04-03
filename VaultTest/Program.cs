// See https://aka.ms/new-console-template for more information
using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using VaultManager;
using VaultManager.Crypto;

//using VaultManager.Models;

Console.WriteLine("Hello, World!");

/*var key = EncryptionManager.LoadKey("test123");

var valueEncryptionManager = new ValueEncryptionManager();

var serializableVault = valueEncryptionManager.LoadVaultFromFile(key);

var vault = new Vault(serializableVault);

vault.PasswordVault.AddPassword(new PasswordEntry(
    Guid.Parse("6D92E24F-7B74-4447-8B84-1907FB36A6F2"), 
    new Uri("https://www.xuri.duckdns.com"),
    "test123",
    "password",
    null,
    null,
    null
));

valueEncryptionManager.SaveVaultFromFile(vault.ToSerializable(), key);*/

var context = new VaultContext();
if (!context.Passwords.Any())
{
    context.Passwords.Add(new Credential()
    {
        Description = "Test Description",
        Password = "Test Password",
        Username = "Test Username",
        Tags = new List<Tag>
        {
            new Tag
            {
                Description = "Test Tag Description",
            }
        },
        WebSide = new WebSide
        {
            Description = "Main Website",
            Domain = new Uri("https://www.xuri.duckdns.com"),
        },
        Notes = "Some notes with \n newlines",
    });
    context.SaveChanges();
}

var passwords = context.Passwords
    .Include(x=>x.WebSide).ToList();
foreach (var credential in passwords)
{
    Console.WriteLine($"{credential.WebSide.Domain}");
}

context.Dispose();
