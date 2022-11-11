using System.Collections.ObjectModel;

namespace Ajiva.PasswordManager.Ui.Maui;

public class PageDescription<T>
{
    public PageDescription(string name, DataTemplate itemTemplate, IDetailView<T> detailView)
    {
        Name = name;
        ItemTemplate = itemTemplate;
        DetailView = detailView;
    }

    public string Name { get; set; }
    public DataTemplate ItemTemplate { get; set; }
    public IDetailView<T> DetailView { get; set; }
}
public interface IDetailView<T> : IView
{
    void SetDetail(T? detail);
    void SetEdit(bool edit);
    void Save();
}
