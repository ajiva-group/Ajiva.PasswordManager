using System.Collections.ObjectModel;

namespace Ajiva.PasswordManager.Ui.Maui;

public class PageDescription<T>
{
    public PageDescription(Func<string?, IEnumerable<T>> searchAction, string name, DataTemplate itemTemplate, IDetailView<T> detailView)
    {
        SearchAction = searchAction;
        Name = name;
        ItemTemplate = itemTemplate;
        DetailView = detailView;
    }

    public Func<string?, IEnumerable<T>> SearchAction;
    public string Name { get; set; }
    public DataTemplate ItemTemplate { get; set; }
    public IDetailView<T> DetailView { get; set; }
}
public interface IDetailView<T> : IView
{
    void SetDetail(T detail);
}
