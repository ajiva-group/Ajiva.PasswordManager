using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Ajiva.PasswordManager.Ui.Maui.Pages;

public abstract class BasePage<T, TV> : ContentPage where T : PageDescription<TV> where TV : class
{
    private readonly T _description;
    private readonly SearchBar _searchBar;
    private readonly Grid _mainContent;
    private readonly VerticalStackLayout _detailsView;
    private readonly CollectionView _collectionView;
    private readonly HorizontalStackLayout _options;
    private readonly Button _edit;
    private readonly Button _cancel;
    private readonly Button _save;
    private readonly VerticalStackLayout _itemListContainer;

    public BasePage(T description)
    {
        _description = description;
        Title = description.Name;

        _searchBar = new SearchBar {
            Placeholder = $"Search {description.Name}...",
            PlaceholderColor = Colors.Gray,
        };
        _searchBar.TextChanged += PerformSearch;
        _itemListContainer = new VerticalStackLayout {
            Children = {
                _searchBar,
                (_collectionView = new CollectionView {
                    ItemTemplate = description.ItemTemplate,
                    SelectionMode = SelectionMode.Single,
                    SelectionChangedCommand = new Command(ItemSelected),
                })
            }
        };


        _cancel = new Button { HorizontalOptions = LayoutOptions.End, Text = "Cancel", TextColor = Colors.Gray };
        _cancel.Clicked += (_, _) =>
        {
            _options.Children.Remove(_cancel);
            _options.Children.Insert(0, _edit);
            _description.DetailView.SetEdit(false);
        };
        _edit = new Button { HorizontalOptions = LayoutOptions.End, Text = "Edit", TextColor = Colors.Gray };
        _edit.Clicked += (_, _) =>
        {
            _options.Children.Remove(_edit);
            _options.Children.Insert(0, _cancel);
            _description.DetailView.SetEdit(true);
        };
        _save = new Button { HorizontalOptions = LayoutOptions.End, Text = "Save", TextColor = Colors.Gray };
        _save.Clicked += (_, _) =>
        {
            _description.DetailView.Save();
            _cancel.SendClicked();
        };

        _detailsView = new VerticalStackLayout {
            Children = {
                (_options = new HorizontalStackLayout {
                    Children = {
                        _edit,
                        _save,
                    },
                }),
                description.DetailView
            }
        };
        
        if (DeviceInfo.Idiom == DeviceIdiom.Phone)
        {
            _mainContent = new Grid {
                _itemListContainer
            };
            var back  = new Button { HorizontalOptions = LayoutOptions.End, Text = "🔙", TextColor = Colors.Gray };
            back.Clicked += (_, _) =>
            {
                _mainContent.Children.Clear();
                _mainContent.Children.Add(_itemListContainer);
            };
            _options.Children.Add(back);
        }
        else
        {
            _mainContent = new Grid {
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(40, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(60, GridUnitType.Star) },
                },
            };
            _mainContent.Add(_itemListContainer, 0, 0);
            _mainContent.Add(_detailsView, 1, 0);
        }

        Content = new StackLayout {
            //HeightRequest = Height,
            Children = {
                new Label { Text = Title, },
                _mainContent
            }
        };
    }

    /// <inheritdoc />
    protected override void OnAppearing()
    {
        base.OnAppearing();
        PerformSearch(null);
    }

    private void PerformSearch(object? sender, TextChangedEventArgs e)
    {
        PerformSearch(e.NewTextValue);
    }

    private void PerformSearch(string? search)
    {
        var items = SearchAction(search).ToList();
        Debug.WriteLine(this, $"Results for {search}: {items.Count}");
        SearchResults = items;
    }

    protected abstract IEnumerable<TV> SearchAction(string? search);

    protected void ItemsChanged()
    {
        PerformSearch(_searchBar.Text);
    }

    private void ItemSelected()
    {
        if (DeviceInfo.Idiom == DeviceIdiom.Phone)
        {
            _mainContent.Children.Clear();
            _mainContent.Children.Add(_detailsView);
        }
        _description.DetailView.SetDetail(_collectionView.SelectedItem as TV);
    }
    
    public IEnumerable<TV> SearchResults
    {
        set => _collectionView.ItemsSource = value;
    }
}
