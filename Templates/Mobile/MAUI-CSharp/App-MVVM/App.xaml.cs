// File: App.xaml.cs
// Application entry point for the MAUI MVVM template. Resolves MainPage via DI or creates it directly.

namespace AppMVVM;

/// <summary>
/// MAUI Application class. Creates the main window, attempting DI resolution of MainPage.
/// </summary>
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // Try to resolve MainPage from DI; fall back to manual construction
        return new Window(Handler?.MauiContext?.Services.GetService<MainPage>() ?? new MainPage(new ViewModels.MainViewModel()));
    }
}
