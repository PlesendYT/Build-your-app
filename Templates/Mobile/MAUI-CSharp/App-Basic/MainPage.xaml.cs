namespace AppBasic;

public partial class MainPage : ContentPage
{
    private int _counter;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object? sender, EventArgs e)
    {
        _counter++;
        CounterLabel.Text = $"Count: {_counter}";
    }
}
