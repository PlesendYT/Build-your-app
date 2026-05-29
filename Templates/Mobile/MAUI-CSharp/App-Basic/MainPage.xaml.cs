// File: MainPage.xaml.cs
// Main page for the Basic MAUI app template. Contains a simple counter button.

namespace AppBasic;

/// <summary>
/// The primary page of the basic MAUI app. Displays a label and increments a counter on button click.
/// </summary>
public partial class MainPage : ContentPage
{
    private int _counter;

    public MainPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Increments the counter and updates the label text.
    /// </summary>
    private void OnCounterClicked(object? sender, EventArgs e)
    {
        _counter++;
        CounterLabel.Text = $"Count: {_counter}";
    }
}
