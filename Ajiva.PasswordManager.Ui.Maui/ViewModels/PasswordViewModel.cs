using Ajiva.PasswordManager.Ui.Maui.Static;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ajiva.PasswordManager.Ui.Maui.ViewModels;

public class PasswordViewModel : INotifyPropertyChanged
{


    private ObservableCollection<PasswordVm> passwords;
    public ObservableCollection<PasswordVm> Passwords
    {
        get
        {
            return passwords;
        }

        set
        {
            passwords = value;
            OnPropertyChanged();
        }
    }

    async void Fetch()
    {
        var vault = StaticData.Vault.Vault;
        var passwords = vault.Passwords.Select(x => new PasswordVm(x.Value, vault)).ToList();

        UpdatePasswords(passwords);

        OnPropertyChanged(nameof(Passwords));
    }

    private void UpdatePasswords(IEnumerable<PasswordVm> locations)
    {
        passwords = new ObservableCollection<PasswordVm>();
        for (int i = locations.Count() - 1; i >= 0; i--)
        {
            passwords.Add(locations.ElementAt(i));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
            handler(this, new PropertyChangedEventArgs(propertyName));
    }

    public PasswordViewModel()
    {
        Fetch();
    }
}
