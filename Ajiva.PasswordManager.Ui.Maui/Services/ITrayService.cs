namespace Ajiva.PasswordManager.Ui.Maui.Services;

public interface ITrayService
{
    void Initialize();

    Action ClickHandler { get; set; }
}
