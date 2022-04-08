using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Ajiva.PasswordManager.Ui.Maui.Pages;

public class BasePage<T, TV> : ContentPage where T : PageDescription<TV>
{
    private readonly T _description;
    private readonly SearchBar _searchBar;
    private readonly Grid _mainContent;
    private readonly StackLayout _detailsView;
    private readonly CollectionView _collectionView;

    public BasePage(T description)
    {
        _description = description;

        _mainContent = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(40, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(60, GridUnitType.Star) },
            },
        };
        _searchBar = new SearchBar
        {
            Placeholder = $"Search {description.Name}...",
            PlaceholderColor = Colors.Gray,
        };

        var itemListContainer = new VerticalStackLayout
        {
            Children =
            {
                _searchBar,
                (_collectionView = new CollectionView
                {
                    ItemsSource = _description.SearchAction.Invoke(null).ToList(),
                    ItemTemplate = description.ItemTemplate,
                    SelectionMode = SelectionMode.Single,
                    SelectionChangedCommand = new Command<TV>(ItemSelected),
                })
            }
        };
        _mainContent.Add(itemListContainer, 0, 0);
        _mainContent.Add(_detailsView = new StackLayout
        {
            Children =
            {
                new HorizontalStackLayout
                {
                    Children = { new Button { HorizontalOptions = LayoutOptions.End, Text = "Edit", TextColor = Colors.Gray } },
                },
                description.DetailView
            }
        }, 1, 0);

        Content = new StackLayout
        {
            //HeightRequest = Height,
            Children =
            {
                new Label { Text = "Welcome to .NET MAUI!" },
                _mainContent
            }
        };
        _searchBar.TextChanged += PerformSearch;
    }

    private void PerformSearch(object? sender, TextChangedEventArgs e)
    {
        var items = _description.SearchAction.Invoke(e.NewTextValue).ToList();
        Debug.WriteLine(this, $"Results for {e.NewTextValue}: {items.Count}");
        SearchResults = items;
    }

    private void ItemSelected(TV obj)
    {
        _description.DetailView.SetDetail(obj);
    }

    /*private void SearchBar_SearchButtonPressed(object obj)
    {
        SearchResults = _description.SearchAction.Invoke(_searchBar.Text);
    }*/

    public IEnumerable<TV> SearchResults
    {
        set => _collectionView.ItemsSource = value;
    }
}
