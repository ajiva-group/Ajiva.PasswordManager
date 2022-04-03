using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VaultManager;

public class VaultContext : DbContext
{
    public string DbPath { get; }

    static VaultContext()
    {
        var vaultContext = new VaultContext();
        vaultContext.Database.Migrate();
        vaultContext.Dispose();
    }
    public VaultContext() : this("u:\\default.ajvault.db"){}
    public VaultContext(string dbPath)
    {
        DbPath = dbPath;
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Credential>()
            .HasOne(b => b.TwoFactor)
            .WithOne(i => i.Credential)
            .HasForeignKey<TowFactor>(b => b.CredentialId);   
        
        modelBuilder.Entity<WebSide>()
            .HasOne(b => b.Settings)
            .WithOne(i => i.WebSide)
            .HasForeignKey<WebSideSettings>(b => b.WebSideId);
    }

    public DbSet<Credential> Passwords { get; set; }
    public DbSet<TowFactor> TowFactors { get; set; }
    public DbSet<Identity> Identities { get; set; }
    public DbSet<WebSide> WebSides { get; set; }
    public DbSet<FileNote> Files { get; set; }
    public DbSet<Tag> Tags { get; set; }
}
public class Credential
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    public WebSide WebSide { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Notes { get; set; }
    
    public TowFactor? TwoFactor { get; set; }
    public ICollection<Tag> Tags { get; set; }
}
public class TowFactor
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    public string Type { get; set; }
    public string Secret { get; set; }
    
    [ForeignKey(nameof(Credential))]
    public Guid CredentialId { get; set; }
    public Credential Credential { get; set; }
    
    public ICollection<Tag> Tags { get; set; }
}
public class Identity
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }

    public string Notes { get; set; }
    public ICollection<Tag> Tags { get; set; }
}
public class WebSide
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    public Uri Domain { get; set; }
    public ICollection<Credential> Passwords { get; set; }
    public WebSideSettings? Settings { get; set; }

    public ICollection<Tag> Tags { get; set; }
}
public class WebSideSettings
{
    public Guid Id { get; set; }

    [ForeignKey(nameof(WebSide))]
    public Guid WebSideId { get; set; }
    public WebSide WebSide { get; set; }
}
public class FileNote
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string Notes { get; set; }
    public byte[] FileData { get; set; }

    public ICollection<Tag> Tags { get; set; }
}
public class Tag
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    public List<Credential> Passwords { get; set; }
    public List<TowFactor> TowFactors { get; set; }
    public List<Identity> Identities { get; set; }
    public List<WebSide> WebSides { get; set; }
    public List<FileNote> Files { get; set; }
}
