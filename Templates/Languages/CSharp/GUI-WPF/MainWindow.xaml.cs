using System.Windows;

namespace GUIWPF;

/// <summary>Main window with a counter and item list.</summary>
public partial class MainWindow : Window
{
    private int _counter;

    /// <summary>Initialises the window components.</summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>Handles button click: increments counter and adds input text to the list.</summary>
    private void OnButtonClick(object sender, RoutedEventArgs e)
    {
        _counter++;
        CounterLabel.Text = $"Count: {_counter}";

        // Add non-empty input to the list box
        if (!string.IsNullOrWhiteSpace(InputBox.Text))
        {
            ItemList.Items.Add(InputBox.Text);
            InputBox.Clear();
        }
    }
}
