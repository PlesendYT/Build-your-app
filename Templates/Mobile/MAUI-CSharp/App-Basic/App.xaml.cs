// File: App.xaml.cs
// Application entry point for the Basic MAUI app template. Creates the main window with MainPage.

namespace AppBasic;

/// <summary>
/// MAUI Application class. Initializes the app and creates the main window hosting MainPage.
/// </summary>
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage());
    }
}
