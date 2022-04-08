using Ajiva.PasswordManager.Ui.Maui.Static;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Ajiva.PasswordManager.Ui.Maui.ViewModels;

public class PasswordViewModel : INotifyPropertyChanged
{
    public ObservableCollection<PasswordVm> Passwords { get; set; } = new ObservableCollection<PasswordVm>();
    public List<PasswordVm> AllPasswords { get; set; } = new List<PasswordVm>();

    void Fetch()
    {
        GetPasswords();
    }

    private void GetPasswords()
    {
        var vault = StaticData.Vault.Vault;
        AllPasswords = vault.Passwords.Select(x => new PasswordVm(x.Value, vault)).ToList();
    }

    private void UpdatePasswords(string? search)
    {
        foreach (var vm in AllPasswords)
        {
            if (Filter(search, vm))
            {
                if (!Passwords.Contains(vm))
                    Passwords.Add(vm);
            }
            else
            {
                Passwords.Remove(vm);
            }
        }

        OnPropertyChanged(nameof(Passwords));
    }

    private static bool Filter(string? search, PasswordVm vm)
    {
        return search is null
               || vm.Description.Contains(search, StringComparison.InvariantCultureIgnoreCase)
               || vm.Username.Contains(search, StringComparison.InvariantCultureIgnoreCase)
               || vm.WebSide.Description.Contains(search, StringComparison.InvariantCultureIgnoreCase)
               || vm.Tags.Any(tag => tag.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase));
    }

    public void Search(string search)
    {
        Debug.WriteLine($"Searching for {search}");
        UpdatePasswords(search);
    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public PasswordViewModel()
    {
        Fetch();
        Search(null);
    }
}
