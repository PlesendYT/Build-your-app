namespace GUIMAUI;

public partial class MainPage : ContentPage
{
    private int _counter;
    private readonly List<string> _items = [];

    public MainPage()
    {
        InitializeComponent();
        ItemView.ItemsSource = _items;
    }

    private void OnButtonClicked(object? sender, EventArgs e)
    {
        _counter++;
        CounterLabel.Text = $"Count: {_counter}";

        if (!string.IsNullOrWhiteSpace(InputEntry.Text))
        {
            _items.Add(InputEntry.Text);
            ItemView.ItemsSource = null;
            ItemView.ItemsSource = _items;
            InputEntry.Text = string.Empty;
        }
    }
}
