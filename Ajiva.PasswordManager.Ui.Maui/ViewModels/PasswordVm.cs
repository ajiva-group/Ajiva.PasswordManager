using VaultManager;

namespace Ajiva.PasswordManager.Ui.Maui.ViewModels;

public class PasswordVm
{

    public PasswordVm()
    {

    }
    public PasswordVm(PasswordEntry argValue, Vault vault)
    {
        Id = argValue.Id;
        Username = argValue.Username;
        Password = argValue.Password;
        Description = argValue.Description;
        Notes = argValue.Notes;

        WebSide = new WebSideVm(argValue.WebSide);
        TwoFactor =argValue.TwoFactor is null ? null : new TwoFactorVm(vault.TwoFactors[argValue.TwoFactor.Value], this);
        Tags = argValue.Tags?.Select(x => new TagVm(x)).ToList() ?? new List<TagVm>();
    }

    public Guid Id { get; set; }

    public string Description { get; set; }

    public WebSideVm WebSide { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Notes { get; set; }

    public TwoFactorVm? TwoFactor { get; set; }
    public List<TagVm> Tags { get; set; }
}
public class TwoFactorVm
{
    public TwoFactorVm(TwoFactorEntry twoFactor, PasswordVm credential)
    {
        Id = twoFactor.Id;
        Description = twoFactor.Description;
        Type = twoFactor.Type;
        Secret = twoFactor.Secret;
        Credential = credential;
        Tags = twoFactor.Tags?.Select(x => new TagVm(x)).ToList() ?? new List<TagVm>();
    }
    public Guid Id { get; set; }
    public string Description { get; set; }

    public string Type { get; set; }
    public string Secret { get; set; }

    public PasswordVm Credential { get; set; }

    public List<TagVm> Tags { get; set; }
}
public class IdentityVm
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
    public List<TagVm> Tags { get; set; }
}
public class WebSideVm
{
    public WebSideVm()
    {

    }
    public WebSideVm(WebSideEntry webSide)
    {
        Id = webSide.Id;
        Description = webSide.Description;
        Domain = webSide.Domain;
        Settings = new WebSideSettingsVm(webSide.Settings);
        FavIcon = new UriBuilder(webSide.Domain) { Path = "/favicon.ico" }.Uri;
        Tags = webSide.Tags?.Select(x => new TagVm(x)).ToList() ?? new List<TagVm>();
    }

    public Guid Id { get; set; }
    public string Description { get; set; }
    public Uri FavIcon { get; set; }
    public Uri Domain { get; set; }
    public List<PasswordVm> Passwords { get; set; }
    public WebSideSettingsVm? Settings { get; set; }

    public List<TagVm> Tags { get; set; }
}
public class WebSideSettingsVm
{
    public WebSideSettingsVm(WebSideSettings settings)
    {
        
    }
}
public class FileNoteVm
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string Notes { get; set; }
    public byte[] FileData { get; set; }

    public List<TagVm> Tags { get; set; }
}
public class TagVm
{
    public TagVm()
    {

    }
    public TagVm(TagEntry tagEntry)
    {
        Id = tagEntry.Id;
        Description = tagEntry.Description;
        Name = tagEntry.Name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
