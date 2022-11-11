using Ajiva.PasswordManager.Ui.Maui.ViewModels;

namespace Ajiva.PasswordManager.Ui.Maui.View;

public partial class PasswordDeteils : ContentView
{
    public PasswordDeteils()
    {
        InitializeComponent();
        BindingContext = this;
    }
    PasswordVm _current;
    public PasswordVm Current
    {
        get => _current; set
        {
            _current = value;
            this.OnPropertyChanged();
        }
    }
}