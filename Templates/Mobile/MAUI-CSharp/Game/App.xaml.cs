// File: App.xaml.cs
// Application entry point for the MAUI Game template. Creates the main window with GamePage.

namespace Game;

/// <summary>
/// MAUI Application class. Initializes the app and creates the main window hosting GamePage.
/// </summary>
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new GamePage());
    }
}
