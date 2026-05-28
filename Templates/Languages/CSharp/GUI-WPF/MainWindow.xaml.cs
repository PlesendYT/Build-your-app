using System.Windows;

namespace GUIWPF;

public partial class MainWindow : Window
{
    private int _counter;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnButtonClick(object sender, RoutedEventArgs e)
    {
        _counter++;
        CounterLabel.Text = $"Count: {_counter}";

        if (!string.IsNullOrWhiteSpace(InputBox.Text))
        {
            ItemList.Items.Add(InputBox.Text);
            InputBox.Clear();
        }
    }
}
