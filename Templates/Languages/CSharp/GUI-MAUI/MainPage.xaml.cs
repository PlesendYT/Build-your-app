namespace GUIMAUI;

/// <summary>Main page with a counter and an item list.</summary>
public partial class MainPage : ContentPage
{
    private int _counter;
    private readonly List<string> _items = [];

    /// <summary>Initialises the page and binds the item list source.</summary>
    public MainPage()
    {
        InitializeComponent();
        ItemView.ItemsSource = _items;
    }

    /// <summary>Handles button click: increments counter and adds entry text to the list.</summary>
    private void OnButtonClicked(object? sender, EventArgs e)
    {
        _counter++;
        CounterLabel.Text = $"Count: {_counter}";

        // Add non-empty input text to the list
        if (!string.IsNullOrWhiteSpace(InputEntry.Text))
        {
            _items.Add(InputEntry.Text);
            // Re-bind to refresh the view
            ItemView.ItemsSource = null;
            ItemView.ItemsSource = _items;
            InputEntry.Text = string.Empty;
        }
    }
}
