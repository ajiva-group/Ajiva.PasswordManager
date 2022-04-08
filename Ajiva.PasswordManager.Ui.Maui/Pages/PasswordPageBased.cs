using Ajiva.PasswordManager.Ui.Maui.Resources.Styles;
using Ajiva.PasswordManager.Ui.Maui.Static;
using Ajiva.PasswordManager.Ui.Maui.ViewModels;

namespace Ajiva.PasswordManager.Ui.Maui.Pages;

public class PasswordPageBased : BasePage<PageDescription<PasswordVm>, PasswordVm>
{
    public PasswordPageBased() : base(new PageDescription<PasswordVm>(
        SearchFilter,
        "Password",
        (DataTemplate)new DefaultLists()["PasswordTemplate"],
        new PasswordDetailsView())
    )
    {
    }

    private static IEnumerable<PasswordVm> SearchFilter(string? arg)
    {
        var vault = StaticData.Vault.Vault;
        var passwordVms = vault.Passwords.Select(x => new PasswordVm(x.Value, vault)).ToList();
        foreach (var passwordVm in passwordVms.Where(passwordVm => Filter(arg, passwordVm)))
        {
            yield return passwordVm;
        }
    }

    private static bool Filter(string? search, PasswordVm vm)
    {
        return search is null
               || vm.Description.Contains(search, StringComparison.InvariantCultureIgnoreCase)
               || vm.Username.Contains(search, StringComparison.InvariantCultureIgnoreCase)
               || vm.WebSide.Description.Contains(search, StringComparison.InvariantCultureIgnoreCase)
               || vm.Tags.Any(tag => tag.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase));
    }
}
public class PasswordDetailsView : VerticalStackLayout, IDetailView<PasswordVm>
{
    
    /// <inheritdoc />
    public void SetDetail(PasswordVm detail)
    {
    }
}
