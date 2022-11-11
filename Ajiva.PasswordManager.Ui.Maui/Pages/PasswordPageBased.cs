using System.Diagnostics.CodeAnalysis;
using Ajiva.PasswordManager.Ui.Maui.Resources.Styles;
using Ajiva.PasswordManager.Ui.Maui.Static;
using Ajiva.PasswordManager.Ui.Maui.ViewModels;
using VaultManager;

namespace Ajiva.PasswordManager.Ui.Maui.Pages;

public class PasswordPageBased : BasePage<PageDescription<PasswordVm>, PasswordVm>
{
    public IVaultService VaultService { get; }

    public PasswordPageBased(IVaultService vaultService) : base(new PageDescription<PasswordVm>(
        "Password",
        (DataTemplate)new DefaultLists()["PasswordTemplate"],
        new PasswordDetailsView(vaultService)))

    {
        VaultService.Vault.Vault.PasswordEntryChanged += VaultOnPasswordEntryChanged;
        VaultService = vaultService;
    }

    private void VaultOnPasswordEntryChanged(PasswordEntry arg1, PasswordEntry arg2)
    {
        ItemsChanged();
    }

    private static bool Filter(string? search, PasswordVm vm)
    {
        return search is null
               || vm.Description.Contains(search, StringComparison.InvariantCultureIgnoreCase)
               || vm.Username.Contains(search, StringComparison.InvariantCultureIgnoreCase)
               || vm.WebSide.Domain.ToString().Contains(search, StringComparison.InvariantCultureIgnoreCase)
               || vm.Tags.Any(tag => tag.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase));
    }

    /// <inheritdoc />
    protected override IEnumerable<PasswordVm> SearchAction(string? search)
    {
        var vault = VaultService.Vault.Vault;
        var passwordVms = vault.Passwords.Select(x => new PasswordVm(x.Value, vault)).ToList();
        foreach (var passwordVm in passwordVms.Where(passwordVm => Filter(search, passwordVm)))
        {
            yield return passwordVm;
        }
    }
}
[SuppressMessage("Interoperability", "CA1416:Plattformkompatibilität überprüfen")]
public class PasswordDetailsView : VerticalStackLayout, IDetailView<PasswordVm>
{
    private readonly Grid _stack;
    private readonly Label _id;
    private readonly Entry _description;
    private readonly Entry _username;
    private readonly Entry _password;
    private readonly Entry _webSide;
    private readonly InputView _notes;

    public PasswordDetailsView(IVaultService vaultService)
    {
        _stack = new Grid
        {
            ColumnDefinitions = {
                new ColumnDefinition { Width = new GridLength(30, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(70, GridUnitType.Star) },
                new ColumnDefinition { Width = GridLength.Auto },
            },
            Padding = new Thickness { Left = 10, Right = 10, Top = 10, Bottom = 10 },
            RowSpacing = 10,
            ColumnSpacing = 20,
            RowDefinitions = {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
            }
        };
        _id = new Label { Text = "IDField", VerticalOptions = LayoutOptions.Center };
        _description = new Entry { Text = "DescriptionField", VerticalOptions = LayoutOptions.Center };
        _username = new Entry { Text = "UsernameField", VerticalOptions = LayoutOptions.Center };
        _password = new Entry { Text = "PasswordField", VerticalOptions = LayoutOptions.Center };
        _webSide = new Entry { Text = "WebSideField", VerticalOptions = LayoutOptions.Center };
        _notes = new Entry { Text = "NotesField", };


        var copyId = new Button { Text = "📋" };
        copyId.Clicked += async (_, _) => await Clipboard.SetTextAsync(_id.Text);
        var togglePassword = new Button { Text = "👁‍" };
        togglePassword.Clicked += (_, _) => { pw = !pw; ApplyPasswordVis(); };

        _stack.Add(new Label { Text = nameof(PasswordVm.Id) + ":", VerticalOptions = LayoutOptions.Center }, 0, 0);
        _stack.Add(_id, 1, 0);
        _stack.Add(copyId, 2, 0);

        _stack.Add(new Label { Text = nameof(PasswordVm.Description) + ":", VerticalOptions = LayoutOptions.Center }, 0, 1);
        _stack.Add(_description, 1, 1);

        _stack.Add(new Label { Text = nameof(PasswordVm.Username) + ":", VerticalOptions = LayoutOptions.Center }, 0, 2);
        _stack.Add(_username, 1, 2);

        _stack.Add(new Label { Text = nameof(PasswordVm.Password) + ":", VerticalOptions = LayoutOptions.Center }, 0, 3);
        _stack.Add(_password, 1, 3);
        _stack.Add(togglePassword, 2, 3);

        _stack.Add(new Label { Text = nameof(PasswordVm.WebSide) + ":", VerticalOptions = LayoutOptions.Center }, 0, 4);
        _stack.Add(_webSide, 1, 4);

        _stack.Add(new Label { Text = nameof(PasswordVm.Notes) + ":", VerticalOptions = LayoutOptions.Center }, 0, 5);
        _stack.Add(_notes, 1, 5);
        SetEdit(false);
        VaultService = vaultService;
    }

    PasswordVm? last;

    /// <inheritdoc />
    public void SetDetail(PasswordVm? detail)
    {
        last = detail;
        Children.Clear();
        if (detail is null) return;
        _id.Text = detail.Id.ToString();
        _description.Text = detail.Description;
        _username.Text = detail.Username;
        //_password.Text = detail.Password;
        _webSide.Text = detail.WebSide.Domain.ToString();
        Children.Add(_stack);
        SetEdit(false);
    }

    bool edit = false;
    bool pw = false;

    public IVaultService VaultService { get; }

    /// <inheritdoc />
    public void SetEdit(bool edit)
    {
        this.edit = edit;
        _description.IsReadOnly = !edit;
        _username.IsReadOnly = !edit;
        _password.IsReadOnly = !edit;
        _webSide.IsReadOnly = !edit;
        if (edit)
            _password.Text = last.Password;
        ApplyPasswordVis();
    }

    private void ApplyPasswordVis()
    {
        if (!edit) _password.Text = pw ? last?.Password : "⚪⚪⚪⚪⚪⚪⚪⚪⚪⚪";
        else _password.IsPassword = pw;
    }

    /// <inheritdoc />
    public void Save()
    {
        
        var newVm = VaultService.Vault.Update(new PasswordEntry()
        {
            Id = Guid.Parse(_id.Text),
            Description = _description.Text,
            Username = _username.Text,
            Password = _password.Text,
            WebSide = new WebSideEntry()
            {
                Domain = new Uri(_webSide.Text)
            },
            Notes = _notes.Text,
        });
        last = new PasswordVm(newVm, VaultService.Vault.Vault);
    }
}
