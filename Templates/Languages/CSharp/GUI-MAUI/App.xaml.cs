namespace GUIMAUI;

/// <summary>MAUI Application entry point. Sets the main page.</summary>
public partial class App : Application
{
    /// <summary>Initialises the app and assigns the main page.</summary>
    public App()
    {
        InitializeComponent();
        MainPage = new MainPage();
    }
}
